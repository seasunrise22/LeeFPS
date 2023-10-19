# LeeFPS
- 개발인원 : 1명
- 역할
  - 전체
## Introduction
유니티로 만들어 본 1인칭 FPS 게임입니다.

하늘에서 내려오는 상자를 부수는 간단한 규칙의 게임입니다.

일정 수 이상의 부수지 못한 상자가 바닥에 닿으면 게임오버 됩니다.
## Development Environment
- Unity 2019.4.30f1
## Screenshots
![스크린샷 2023-10-19 190041](https://github.com/seasunrise22/LeeFPS/assets/45503931/66226c31-28f6-45bb-a64b-106f70229559)
![스크린샷 2023-10-19 190449](https://github.com/seasunrise22/LeeFPS/assets/45503931/808e73cd-feb7-423c-b9b1-195510be6343)
![스크린샷 2023-10-19 190332](https://github.com/seasunrise22/LeeFPS/assets/45503931/cc0313c8-f2a1-4e3c-8eef-15c4ea8a2b40)
![스크린샷 2023-10-19 190407](https://github.com/seasunrise22/LeeFPS/assets/45503931/9d4fe7d3-d823-42f4-af44-66f761475c90)
## Code Preview
### 게임의 흐름을 제어하는 GameManager, 스테이지 난이도를 제어하는 LevelManager
각 Manager 파일은 두 개 이상 존재해서는 안 되므로 싱글턴으로 구현
```c#
GameManager.cs

// 게임 시작과 동시에 싱글턴을 구성
    private void Awake()
    {
        // 싱글턴 변수 instance가 비어 있는가?
        if(instance == null)
        {
            // instance가 비어 있다면(null) 그곳에 자기 자신을 할당
            instance = this;
        }
        else
        {
            // instance에 이미 다른 GameManager 오브젝트가 할당되어 있는 경우
            // 씬에 두 개 이상의 GameManager 오브젝트가 존재한다는 의미
            // 싱글턴 오브젝트는 하나만 존재해야 하므로 자신의 게임 오브젝트를 파괴.
            Debug.LogWarning("씬에 두 개 이상의 게임 매니저가 존재합니다!");
            Destroy(gameObject);
        }
    }
```
Update 함수로 매 프레임 시간 제어와 게임오버 여부 체크
```c#
GameManager.cs

private void Update()
    {
        // 타이머 감소 및 다음 스테이지로의 전개. LevelManager.cs와 연결.
        if (!isLvUp)
        {
            timerText.text = (timeLeft).ToString("0.00");
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                isLvUp = true; // 타이머가 0이 되면 if문이 더 이상 동작하지 않게 하기 위해.
                timeLeft = 0f;
                LevelManager.instance.SetStage(isLvUp);
            }
        }

        // 게임오버 트리거가 발동됐을 경우 아래 작업 수행
        if (isGameover)
        {
            gameoverText.SetActive(true);

            // 최고점수 가져오기
            if (PlayerPrefs.GetInt("bestScore") < LevelManager.instance.stageNum)
                PlayerPrefs.SetInt("bestScore", LevelManager.instance.stageNum);
            bestStage.text = "Best Stage : " + PlayerPrefs.GetInt("bestScore");

            Time.timeScale = 0;

            // R키를 눌러서 씬을 다시 불러오라 = 게임 다시 시작
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Game");
                Time.timeScale = 1;
            }
        }
    }
```
LevelManager 로 스테이지 난이도 조정
```c#
LevelManager.cs

public void SetStage(bool isLvUp)
    {
        if (isLvUp)
        {
            // 스테이지가 바뀌면 기존에 존재하던 상자는 모두 파괴.
            for (int i = 0; i < CrateGenerator.instance.existCrates.Count; i++)
                CrateGenerator.instance.existCrates[i].GetComponent<Target>().Die();

            // 타이머 표시
            StartCoroutine("interTimerCoroutine");
        }       
    }

    // 스테이지 넘어갈 때 표시할 타이머. 3초.
    IEnumerator interTimerCoroutine()
    {
        CrateGenerator.instance.isGen = false; // 상자 생성 중단.

        interTimerObj.SetActive(true);

        interTimerText.text = (interTime).ToString("0"); // 3
        yield return new WaitForSeconds(1f);

        interTime--;
        interTimerText.text = (interTime).ToString("0"); // 2
        yield return new WaitForSeconds(1f);

        interTime--;
        interTimerText.text = (interTime).ToString("0"); // 1
        yield return new WaitForSeconds(1f);

        interTimerObj.SetActive(false);
        interTime = 3f;
        
        CrateGenerator.instance.genRate = CrateGenerator.instance.genRate / 1.2f; // 상자 떨어지는 간격 조정
        CrateGenerator.instance.Start(); // isGen = true로 해서 상자 생성 재개, existCrates 리스트 초기화.

        // 게임 시간 다시 흐르도록.
        GameManager.instance.timeLeft = 30f;
        GameManager.instance.isLvUp = false;

        // 스테이지 UI 갱신.
        stageNum++;
        stageText.text = "Stage : " + stageNum;
    }
```
### 애니메이터로 총기 애니메이션 제어
![스크린샷 2023-10-19 192303](https://github.com/seasunrise22/LeeFPS/assets/45503931/0c5b0317-d9b6-4375-bff3-c418649777c3)
### 사격 로직
```c#
Gun.cs

private void Update()
    {
        if(Time.timeScale == 1)
        {
            GameManager.instance.UpdateAmmo(currentAmmo, maxAmmo, extraCurrentAmmo, extraMaxAmmo);

            // 제장전 상태
            if (isReloading)
                return;

            // 사격
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                // 사격 버튼을 눌렀을 때 현재 총알이 존재한다면
                if (currentAmmo > 0)
                {
                    nextTimeToFire = Time.time + fireRate;
                    Shoot();
                }
                // 사격 버튼을 눌렀을 때 현재 총알이 존재하지 않는다면 
                else if (currentAmmo == 0 && extraCurrentAmmo == 0 && Input.GetButtonDown("Fire1"))
                {
                    gunAudio.clip = emptyAmmo;
                    gunAudio.Play();
                    return;
                }

            }
```
### 상자생성
```c#
CrateGenerator.cs

public void Start()
    {
        existCrates = new List<GameObject>();
        isGen = true; // 상자를 생성하라.
    }    

    private void FixedUpdate()
    {      
        if(Time.time > nextGenTime)
        {
            if(isGen)
            {
                GenerateCrate();
                nextGenTime = Time.time + genRate;
            }            
        }   
    }

    void GenerateCrate()
    {
        // 위치 랜덤 생성
        Vector3 randomPos = new Vector3(
            Random.Range(-8f, 15f), // minX, maxX
            Random.Range(25f, 30f), // minY, maxY
            Random.Range(3f, 25f)); // minZ, maxZ

        // 회전 랜덤 생성
        Vector3 randomRot = new Vector3(
            Random.Range(0f, 45f), // minX, maxX
            Random.Range(0f, 45f), // minY, maxY
            Random.Range(0f, 45f)); // minZ, maxZ

        existCrates.Add(Instantiate(setCrates[Random.Range(0, setCrates.Length)], randomPos, Quaternion.Euler(randomRot)));
    }
```
