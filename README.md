# ClubManagement

## 📌 프로젝트 개요

- **프로젝트명**: ClubManagement  
- **목적**:  
  볼링 동호회 운영 시, 모임 참석 및 회비 납부 정보를 수기로 관리하면서 발생하는 불편함을 해결하고자,  
  모임 출석률과 회비 납부 현황을 확인할 수 있는 **모임 관리 프로그램**을 개발.  

- **주요 기능**:
  - 회원 정보 관리
  - 정기/비정기 모임 출석 관리
  - 회비 납부 및 미납 내역 관리
  - 게임 기록 및 스코어보드 관리
  - 기획서 바로가기 : https://github.com/park0684/ClubManagement/blob/master/%EA%B8%B0%ED%9A%8D%EC%84%9C.md
---

## 📅 개발 기간

- **시작일**: 2025년 3월 18일  
- **진행 상태**: 개발 중

---

## 👥 개발 인원

- 1인 개발

---

## 🛠 사용 기술

- **언어/런타임**: C# (.NET Framework 4.8)  
- **UI 프레임워크**: Windows Forms (WinForms)  
- **DBMS**: Microsoft SQL Server  
- **디자인 패턴**: MVP (Model - View - Presenter)

---

## 📁 프로젝트 구조
| 프로젝트                 | 설명                                 |
|--------------------------|--------------------------------------|
| `ClubManagement`         | 메인 화면, 사이드 메뉴 구성            |
| `ClubManagement.Members` | 회원 조회, 추가, 수정, 회비 등 관리    |
| `ClubManagement.Games`   | 모임/경기 조회, 참가자 관리            |
| `ClubManagement.Common`  | 공통 클래스, 설정, 상수 등 모음        |

---

## 기능 설명

<## 📋 회원 관리

회원의 정보와 회비 납부 현황을 등록 및 조회할 수 있는 메뉴입니다.

<details>
<summary><strong>1. 회원 조회</strong></summary>

- 회원 조회 조건은 **상태**, **모임 유형 제외**, **기간 설정**이 있습니다.
- **회원 상태**:
  - `가입`, `탈퇴`, `열외` 중 선택 가능. 선택된 상태만 조회되며 `제외`를 체크시 선택된 유형은 검색되지 않습니다
  - `전체`를 선택한 경우 `열외` 조건은 적용되지 않습니다.
- **모임 유형 제외**:
  - 최대 **2개까지 중복 선택** 가능  
  - 선택된 유형은 **참가 수에서 제외**됩니다.
- **기간 조건**:
  - 기준: `가입일`, `탈퇴일`, `게임 참가일`  
  - `게임 참가일` 선택 시, 해당 기간 내 게임에 **참가한 회원**만 조회됩니다.
  ![Image](https://github.com/user-attachments/assets/cb7ca7da-cacc-406c-9d48-858112d94e0b)
</details>

<details>
<summary><strong>2. 회비 관리</strong></summary>

- 각 회원별 **납부 대상**, **납부**, **미납**, **면제 횟수**를 조회할 수 있습니다.  
- 검색 기능을 통해 **회원 이름으로 특정 회원 조회**가 가능합니다.  
- 납부 대상은 **회원의 가입월부터 현재 월까지**를 기준으로, **매월 1회씩 자동 집계**됩니다.  
- 미납 횟수는 아래의 방식으로 계산됩니다:
  - `미납 = 납부 대상 - (납부 + 면제)`
  - 단, `납부 > 납부 대상`인 경우에는 **미납 = 0** (선납 처리)

</details>
  

