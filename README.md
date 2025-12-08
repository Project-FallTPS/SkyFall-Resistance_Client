<div align="center">
<h2>SKYFALL : Resistance 🛸</h2>

하늘에서 떨어지며 적들을 쓸어버리고, 지상에 도달하여 보스를 쓰러뜨리는 신개념 플라이트 슈팅 게임, SKYFALL : Resistance입니다!<br>
해당 프로젝트는 SKKU Com2us SAY 1기에서 진행한 프로젝트입니다.🍀

#### ↓↓↓↓↓ 아래 이미지를 클릭하면 SKYFALL : Resistance 플레이 영상을 유튜브에서 보실 수 있습니다. ↓↓↓↓↓
[![플레이 영상](https://img.youtube.com/vi/EEhaOh24GlY/maxresdefault.jpg)](https://www.youtube.com/watch?v=EEhaOh24GlY)<br>
</div><br>

## 목차
  - [개요](#개요) 
  - [게임 설명](#게임-설명)
  - [사용 기술](#사용-기술)
  - [게임 플레이](#게임-플레이)
<br>

## 개요
| **프로젝트 명** | SKYFALL : Resistance |
|:---:|:---:|
| **프로젝트 기간** | 2025.05 - 2025.06 |
| **팀원** | 박미르, 박우영, 차수민, 최민규 |
| **기술 스택** | <img src="https://img.shields.io/badge/Unity-6000.0.58f2-000000?style=for-the-badge&logo=unity" height="25"> <img src="https://img.shields.io/badge/C%23-512BD4?style=for-the-badge&logo=csharp&logoColor=white" height="25"> <img src="https://img.shields.io/badge/Github-181717?style=for-the-badge&logo=github" height="25"> |
| **플랫폼 및 장르** | <img src="https://img.shields.io/badge/Platform-Windows-lightgrey?style=for-the-badge" height="25"> <img src="https://img.shields.io/badge/Genre-3D%20Flying%20tps-red?style=for-the-badge" height="25"> |

<br>

## 게임 설명
|![image](https://github.com/user-attachments/assets/71a067e3-72a4-42cc-bbce-2ac70f75a829)|![image](https://github.com/user-attachments/assets/40474d49-b4ed-48d9-875c-ed3e71e27331)|
|:---:|:---:|
|로비 화면|인게임 화면|

#### **하늘에서 지상까지, 마주하는 모든 적들을 섬멸하라!** 🛸<br>

SKYFALL : Resistance는 무한한 하늘에서 시작하여 지상의 보스전으로 이어지는 고속 낙하 액션 TPS 게임입니다. 
플레이어는 저항군의 일원이 되어, 낙하하는 동안 적들의 포화를 뚫고 지상에 착륙하여 강력한 보스를 쓰러뜨려야 합니다. 
<br><br>

#### 1. 고속 낙하 액션  ☁️
- 끊임없이 추락하는 고속 상황 속에서, 하늘을 뒤덮는 적 기계 군단과 장애물을 피하며 전투를 펼칩니다.
- **카타나**와 **총**을 자유롭게 스위칭하며 스타일리시한 액션을 구사할 수 있습니다.<br>

#### 2. 다채로운 몬스터 👾
- 단순히 플레이어를 향해 다가오는 몬스터뿐만 아니라 자폭, 지원 사격, 베지어 곡선 이동 등 다양한 행동 양식을 가진 적들이 등장하여 긴장감을 늦출 수 없게 합니다. <br>

#### 3. 전략적 보스 레이드 🤖
- 지상에 착륙한 후에는 거대 로봇 보스와의 치열한 전투가 기다리고 있습니다.
- 보스의 **페이즈**별로 변화하는 강력한 패턴을 파악하고, 지형지물을 활용해 공격을 회피하며 약점을 공략해야 합니다.<br>
  
<br>

## 사용 기술
### 1. Strategy Pattern for Player's Attack
- 카타나(근접)와 총(원거리)으로 실시간 전환되는 플레이어의 공격 방식을 유연하게 관리하기 위해 전략 패턴을 적용했습니다.
- 공격 로직을 IAttackStrategy 인터페이스로 추상화하고, 무기 교체 시 전략 객체만 교체함으로써 조건문 없이 깔끔한 공격 로직을 구현했습니다.
- 이를 통해 입력 처리부와 공격 구현부를 분리하여 코드의 결합도를 낮추고 확장성을 확보했습니다.
<br>

### 2. Object Pooling
- 고속 비행 전투 특성상 끊임없이 생성되고 파괴되는 적, 추락 잔해, 투사체 오브젝트들을 효율적으로 관리하기 위해 오브젝트 풀링을 도입했습니다.
- 빈번한 Instantiate와 Destroy 호출로 인한 메모리 단편화 및 빈번한 GC 호출로 인한 오버헤드를 방지할 수 있었습니다.
<br>

### 3. Enemy FSM based on State Pattern
- 몬스터의 상태(추적, 공격, 피격, 사망) 전이를 명확하게 관리하기 위해 **상태 패턴 기반의 FSM**을 설계했습니다.
- 각 상태를 클래스로 캡슐화하여 코드의 결합도를 낮추고, 상태 전이 로직의 가독성과 유지보수성을 높였습니다.
<br>

### 4. Boss AI based on Behavior Tree
- 보스의 체력에 따른 페이즈 전환과 복잡한 공격 패턴(직사, 곡사, 돌진, 레이저)의 발동 조건을 효과적으로 관리하기 위해  **행동 트리**를 도입했습니다.
- Unity Behavior 에셋을 활용하여 시각적으로 로직을 설계했으며, 플레이어와의 거리나 장애물 여부 등 다양한 조건을 유연하게 판단하도록 구현했습니다.
- 복잡한 보스 AI 로직을 간편하게 설계할 수 있었고, Unity에 내장된 Behavior Graph를 통해 간편한 테스트와 디버깅을 수행할 수 있었습니다.
<br>

### 5. Mathematical Projectile Logic (Bezier & Hermite)
- 단순한 추적을 넘어 게임의 재미를 더하기 위해 다양한 이동 및 공격 알고리즘을 적용했습니다.
- **베지어 곡선**을 활용하여 몬스터의 자연스러운 곡선 이동을 구현했고, **에르미트 곡선**을 활용하여 엄폐물 뒤에 숨어있 플레이어를 타격할 수 있는 **곡사 투사체 로직**을 설계했습니다.
<br>

<br>

## 게임 플레이
### 조작법
| 구분 | 동작 | 입력 키 (Input) |
| :---: | :---: | :---: |
| **이동** | 플레이어 이동 | <kbd>W</kbd> <kbd>A</kbd> <kbd>S</kbd> <kbd>D</kbd>|
| **시점 이동** | 카메라 시점 이동 | <kbd>Mouse Movement</kbd> |
| **무기 변경** | 카타나로 변경 / 총으로 변경 | <kbd>1</kbd> <kbd>2</kbd> |
| **공격** | 카타나 기본 공격 / 카타나 대쉬 공격 / 총 공격 | <kbd>Mouse Left Click</kbd> |
| **스프린트** | 이동속도 증가 | <kbd>Left Shift</kbd> |
<br>

### 주요 화면
#### 1. 추락 페이즈
|기본 공격|대쉬 공격|총 공격|추락 잔해|
|:---:|:---:|:---:|:---:|
|![image](https://github.com/user-attachments/assets/346ffc4f-b03a-4daa-a1b0-729f96b9c5a6)|![image](https://github.com/user-attachments/assets/6039751b-5fd0-40db-9f92-c0b0a4d00522)|![image](https://github.com/user-attachments/assets/b6351d5f-6c58-4ff5-85f9-476e7b763198)|![image](https://github.com/user-attachments/assets/0513bfbf-2617-43c3-843f-4e9ca8c4a2d9)|
|카타나를 전방에 휘두르는 기본 공격입니다.|타겟팅된 적을 향해 빠르게 대쉬하며 적을 베어내는 공격입니다.|적을 처치하고 드랍된 총을 획득하여 3발의 탄환을 발사하는 공격입니다.|랜덤하게 스폰되어 떨어지는 크고 작은 잔해들을 피해야 합니다.|
<br>

#### 2. 보스전
|보스전 진입|1페이즈 곡사 패턴|2페이즈 돌진 패턴|3페이즈 레이저 패턴|
|:---:|:---:|:---:|:---:|
|![image](https://github.com/user-attachments/assets/a5fdf8fb-80b3-43b4-a170-a64eb13333c7)|![image](https://github.com/user-attachments/assets/46123293-4035-4a40-8625-e0e57a22f0ba)|![image](https://github.com/user-attachments/assets/570eafed-958f-458e-847d-47b53b63a2fa)|![image](https://github.com/user-attachments/assets/18d4b594-9297-4074-8c33-3491ee152b04)|
|무인도에서 로봇형 보스와 보스전을 진행합니다.|엄폐에 숨어있는 플레이어를 향해 엄폐를 피해 곡선 궤적의 미사일을 발사하는 패턴입니다.|전조 이펙트 후 플레이어를 향해 돌진하는 패턴입니다.|매우 짧은 전조 이펙트 후 플레이어에게 레이저를 발사하는 패턴입니다.|


