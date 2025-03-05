# 주제

## UE의 GameplayAbilitySystem(GAS)을 모방 하여 제작 한 시스템 UnityActionSystem  
- UE의 GAS Plugin은 컴포넌트 기반 아키택쳐(CBD)와 이벤트 기반 프로그래밍, 데이터 중심 설계의 조합으로 
캐릭터의 기능과 이 기능의 상호작용을 데이터 중심적으로 관리하며, 재사용성이 높으면서도 확장성이 좋은 프레임 워크 임.  
- 본 프로젝트는 GAS에서 캐릭터의 행동과 이를 제어하는 시스템을 모방하여 제작하였음.
 
- 캐릭터의 행동을 카테고리별로 정의하고 이를 Component로 적용하여 데이터에 해당하는 변수들을 세팅하도록 개발.
- 각 Action은 특정 상태값을 가지며, 상태에 따라 실행 로직이 달라지는 상태 패턴 적용.
수치 와 상태 값만 바꿔 밸런싱이 가능한 데이터 중심적 설계 적용.
- 캐릭터의 행동에 의한 상호작용을 상태 보유 여부에 따라 처리하도록 개발.  

# 작업 기간 : 2025/02/25 ~ 2025/03/02 (6일)
## 환경 : Unity6 / VS2022

![Example Image](./ReadmeResource/Total.gif)

## 주요 Class UML
### Character
![Example Image](./ReadmeResource/Character%20UML.png)
### Action
![Example Image](./ReadmeResource/Action%20UML.png)
### Action Effect
![Example Image](./ReadmeResource/ActionEffect%20UML.png)

## Action이 ActionSystem을 통해 처리 되는 기본 로직


- StartActionByTag  // Action의 Unique성을 알 수 있는 Tag로 Action 실행  
  - if isCanStart == false // Start 하려는 Tag의 Action 실행 가능 여부 확인인
    -  return    
  - stop actions by start action cancel tags  // Start 하려는 action의 tag에서 취소하도록 정의 한 tag들의 action 취소 처리
  - start action  // 새로 들어온 aciton 실행   
  - if AutoStopAfterOnce == true // 실행 후 바로 해제가 필요한 action은 해제 처리  
    - StopAction  

# 주요 기능 요약
## Player의 현재 Active 된 GameplayTag들을 기반으로 상호작용 및 Action을 관리
## ex)  Defence중 Attack을 하지 못하게 설정 하는 Case  
## Defence 중에는 Status_Defencing이 켜져있음.
## Attack을 실행하려고 체크 시 Attack에 Blocked로 설정 해 둔 Status_Defencing에 의해 Start를 못하도록 Block

![Example Image](./ReadmeResource/Defence.png)![Example Image](./ReadmeResource/Attack%20Base.png)  

# Action
![Example Image](./ReadmeResource/Action.png)  
## 로직
- Start Action
  - Set GrantsTags by Action Grants Tags // 실행 Action에 정의 된 실행 주체에 부여해야 될 Tag 부여
  - Set IsRunning Action // Action Running 설정
  - Set Cooltime // 실행으로 인한 Cooltime 적용
  - Apply Start animation Datas // Action 실행 시 설정정 해 줘야 되는 Animation Data 설정
  - if Action Implement Apply Action Effects // 부여해줘야 되는 ActionEffect가 존재 시
    - Interface.ApplyActionEffects // 부여돼 있는 ActionEffect를 Target에게 부여
  - if Auto Stop == true  // 실행 후 바로 중지 필요 시
    - Stop Action // Action 중지

- Stop Action  
  - Unset GrantsTags by Action Grants Tags  // 부여 했 던 Tag 회수
  - Set Is Not Running                      // Action Not Running 설정
  - Apply Stop animation Datas              // Stop 시 설정 해 줘야 되는 Animation Data 설정

## 주요 Value
- Activation Tag : Action의 Unique 확인용 Tag 해당 Tag를 이용해 Action 적용 가능.  
- Grants Tags : 해당 Action 소유주에게 부여 할 Tags 정의.  
- Cancel Tags : Action 수행 시 활성화 돼 있는 Action을 정지 시킬 Tags 정의.  
- Blocked Tags : 해당 Action 실행 전 Blocked Tags로 정의된 Tag를 Action 소유주가 보유 시 해당 Action 실행 불가.  
- AutoStart : AddAction시 바로 시작 여부.  
- AutoStopAfterOnce : 실행 직후 해제 여부.  
- ApplyActionEffects : 특정 Target에 ActionEffects를 적용할 List 적용 할 Action은 IApplyActionEffectsInterface 상속 필요.  
- Cool Time : 각 Action  별 쿨타임 설정. 쿨타임 중에는 스킬 사용 불가.  
- Start Animation Datas, Stop Animation Datas : 각 상황 별 실행, 중지 애니메이션 데이터 세팅을 위한 내용 정의.


## 생성 Action
![Example Image](./ReadmeResource/CreatedAction.png)  
- Action_Attack : Mouse 우클릭 : 공격 (Action_RangedAttack상세 원거리 공격 적용)  
- Action_Defence : Mouse 좌클릭 : 방어  
- Action_Skill1 : Q : 본인 체력 감소 확인용 스킬 (출혈 효과 적용)  
- Action_Skill2 : E : 무적 효과 적용  
- Action_Dash : L Shift : 무적 효과 적용  

# ActionEffect
![Example Image](./ReadmeResource/ActionEffect.png)  
## 로직
- Start Action
  - if duration > 0
    - Start call execute stop action Coroutine
  - execute period Effect
  - if period > 0
    - Start call execute period Effect Coroutine


## 주요 Value
기본 Action을 상속받아 위 서술한 부분은 생략  
- Duration : ActionEffect 를 적용할 시간  
- Period : 해당 ActionEffect를 적용할 주기  
즉 Duration 동안 Period 가 지날 때 마다  ActionEffect적용  
- 적용 횟수 : Duration / Period  

## 생성 ActionEffect
![Example Image](./ReadmeResource/CreatedActionEffect.png)  
- ActionEffect_Base : 특정 시간, 주기 동안 어떤 효과를 적용 시킬 지 설정 가능  
- ActionEffect_Bleeding : 출혈 효과 : Target의 체력을 dot damage로 감소  
- ActionEffect_Invincibility : 무적 효과 : 적용 Target의 체력 감소 효과를 무시  

# UI
![Example Image](./ReadmeResource/Main.png)
- Unity InputSystem에 Bind 된 Key 설명 UI  
- 캐랙터들의 name과 HP Update를 적용 
- HealthSystem에서 HP Update가 일어 날 때 처리 되도록 System.Action에 Add Listner 하여 Event 기반 Update를 이용해 UI를 Update.
![Example Image](./ReadmeResource/UI%20Code2.png)
![Example Image](./ReadmeResource/UI%20Code3.png)
![Example Image](./ReadmeResource/UI%20Code1.png)  
- 현재 캐릭터에게 부여된 Tag들을 좌측에 표시  
- 스킬 쿨타임 UI : 남아있는 시간을 확인하며 UI 갱신
![Example Image](./ReadmeResource/Skill%20Cooltime%20UI.gif)

# 프로젝트 링크
# [Github](https://github.com/yoon20002000/UnityGameplayActionSystem)