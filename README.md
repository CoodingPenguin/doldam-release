# doldam-for-release
- `성민:UI` `소현:게임시스템` `석현:그래픽` `벼리:기획`
- 목표 출시날짜 : 7월 말

------

## 2019.07.05 박소현
- 프로젝트 생성 및 기존에 있던 Sprites들만 불러옴
- Sprite 설정에서 Filter Mode는 항상 `Point(no filter)`로
- 토론이 필요한 상황은 Issue가 아니라 카카오톡에

------

## 2019.07.07 박성민
### UI List ( 더 필요한 UI가 있다면 기재 바람 )
1. Start Scene
   1. 시작화면 logo
   2. 게임 시작 Btn
   3. 종료 Panel ( Back버튼을 누르면 Pop Up 되는 Panel )
   4. ( 추가 예정 ) 설정, 크레딧, Dev Btn
2. GameScene
   1. 일시정지 버튼 - 일시정지 Panel
   2. 게임오버 Panel
   3. Score ( TextMashPro를 이용 )
3. 설정, 크레딧, Dev
   1. Text ( Page 형식? 아니면 Scroll 형식? )

------

## 2019.07.08 박소현
- 게임 시스템 Script 다 추가
- 상수가 많아서 건드릴 수가 없음. 즉, 거의 기존의 스크립트를 사용
- 항상 Git Pull한 다음에 수정 작업을 합시다!

------

## 2019.07.10 박소현
- `Resources.AllLoad("Path String");`을 쓰려면 Assets을 Resources 폴더 안에다가 넣어야 함
- Trigger가 왠지 작동을 안함
- 그리고 Animation 끊김 현상 있음
- 기존 프로젝트는 괜찮은데 새로 판 프로젝트만 그런 것 같음

------

## 2019.07.11 박소현
- Trigger를 인식하려면 한 쪽이 RigidBody를 켜야함
    - 근데 2D의 경우 RigidBody2D를 사용해야 하고
    - Gravity = 0으로 해놔야 오브젝트가 아래로 안 꺼짐
- 애니메이션이 자꾸 끊김
    - 오브젝트가 배경에 파뭍여서 그럼
        - sortingLayer를 설정하든가
        - 배경을 더 뒤로 빼던가 해야함
    - 안티얼라이싱을 끔
        - 공이 일정속도 이상이 되면 거꾸로 굴러가는 현상 발생
        - 샘플링 잘못되서 그런 듯 -> 안티얼리이싱을 아에 끔