# Documento de diagramas <!-- omit from toc --> 

> 💡 Objetivo do documento: documentar em formato de diagrama os sistemas implementados.
> Esse documento deve descrever de forma diagramal as **interações que cada interface e classes** presentes do sistema utilizam, de forma a ter uma visão clara das estruturas e **propiciar futuras otimizações e refatoração de código**.

Cada diagrama descrito nesse documento deve exibir apenas as interfaces, classes e métodos pertinentes ao sistema descrito.

Summary

- [Sistema de criação de intenções e ações](#sistema-de-criação-de-intenções-e-ações)
- [Grid manager validation system](#grid-manager-validation-system)
  - [Novo sistema de validação do grid dependendo a intenção que a unidade quer desempenhar](#novo-sistema-de-validação-do-grid-dependendo-a-intenção-que-a-unidade-quer-desempenhar)
- [Diagramas de sistemas antigos](#diagramas-de-sistemas-antigos)
  - [Antigo sistema de validação do grid dependendo das intenções tomadas](#antigo-sistema-de-validação-do-grid-dependendo-das-intenções-tomadas)

# Sistema de criação de intenções e ações

```mermaid
classDiagram
direction LR

class UnitActionsView

class UnitActionsFactory {
    + GridUnitAction Get(UnitActionsEnum action)
}

class IAPActor {
    <<interface>>
}

class IAction {
    <<interface>>
    + void Execute()
}

class IIntent {
    <<interface>>
    - ExecuteImmediatly
    + IAction Create()
}

class IAPIntent {
    <<interface>>
    - ActionPointsCost
}

subgraph one
    class UnitSelectionMono
    class IUnitWorldGridManager
    class IWorldCursor
    class IProjectileFactory
end

class DependencyContainer

class UnitIntent

UnitIntent --> IAPIntent : implements

IIntent <-- IAPIntent : extends

class StepMovementAction
StepMovementAction ..> UnitSelectionMono
StepMovementAction ..> IUnitWorldGridManager
StepMovementAction ..> IWorldCursor

class SpinUnitAction
SpinUnitAction ..> UnitSelectionMono

class ShootAction
ShootAction ..> UnitSelectionMono
ShootAction ..> IUnitWorldGridManager
ShootAction ..> IWorldCursor
ShootAction ..> IProjectileFactory

class ThrowGrenadeAction
ThrowGrenadeAction ..> UnitSelectionMono
ThrowGrenadeAction ..> IUnitWorldGridManager
ThrowGrenadeAction ..> IWorldCursor
ThrowGrenadeAction ..> IProjectileFactory

class MeleeAttackAction
MeleeAttackAction ..> UnitSelectionMono
MeleeAttackAction ..> IUnitWorldGridManager
MeleeAttackAction ..> IWorldCursor

class InteractAction
InteractAction ..> UnitSelectionMono
InteractAction ..> IUnitWorldGridManager
InteractAction ..> IWorldCursor

UnitActionsView --> UnitActionsFactory : call Get(UnitActionsEnum action)
IAPActor --> UnitIntent : call Create()

StepMovementAction --> IAction : implements
SpinUnitAction --> IAction : implements
ShootAction --> IAction : implements
ThrowGrenadeAction --> IAction : implements
MeleeAttackAction --> IAction : implements
InteractAction --> IAction : implements

UnitActionsFactory --> UnitIntent : instantiate(UnitActionsEnum)
UnitActionsFactory ..> DependencyContainer : depends

UnitIntent --> StepMovementAction : instantiate
UnitIntent --> SpinUnitAction : instantiate
UnitIntent --> ShootAction : instantiate
UnitIntent --> ThrowGrenadeAction : instantiate
UnitIntent --> MeleeAttackAction : instantiate
UnitIntent --> InteractAction : instantiate

```

---

# Grid manager validation system

Novo sistema de validação do grid dependendo a intenção que a unidade quer desempenhar
---

```mermaid
classDiagram
direction BT

class UnitWorldGridValidator {
    + UnitWorldGridValidator WhereIsNotEmpty()
    + UnitWorldGridValidator WhereIsEmpty()
    + UnitWorldGridValidator WithRange()
    + UnitWorldGridValidator WhereIsUnit()
    + UnitWorldGridValidator WhereUnit()
    + UnitWorldGridValidator Apply()
    + UnitWorldGridValidator Apply()
}

class WorldGridManager~T~{
    IWorldGridXZ~T~ Grid
    + WorldGridManager<T> ApplyValidator(params IGridValidation~T~[] gridValidations)
    + void ResetValidation()
}

class UnitWorldGridManager {
    UnitWorldGridValidator Validator()
}

UnitWorldGridManager --|> WorldGridManager~T~ : implements

class IGridValidation~CellValue~ {
    <<interface>>
    + bool IsAvailable(GridCellXZ<CellValue> cell)
}

class GridUnitAction {
    + ApplyValidation()
}

class UnitActionsFactory

GridUnitAction ..> UnitWorldGridValidator

UnitActionsFactory --> GridUnitAction : instantiate(UnitWorldGridValidator)
UnitActionsFactory --> UnitWorldGridValidator : build

UnitWorldGridValidator <|-- GridUnitAction : Apply

UnitWorldGridValidator --> WorldGridManager : ApplyValidator(IGridValidation[])
UnitWorldGridManager --> UnitWorldGridValidator : instantiate
UnitWorldGridValidator --> IGridValidation : instantiate

UnitActionsFactory ..> UnitWorldGridManager

```

# Diagramas de sistemas antigos

---

Antigo sistema de validação do grid dependendo das intenções tomadas
---

```mermaid
classDiagram
direction RL

class UnitWorldGridValidator {
    + UnitWorldGridValidator WhereIsNotEmpty()
    + UnitWorldGridValidator WhereIsEmpty()
    + UnitWorldGridValidator WithRange()
    + UnitWorldGridValidator WhereIsUnit()
    + UnitWorldGridValidator WhereUnit()
    + UnitWorldGridValidator Apply()
    + UnitWorldGridValidator Apply()
}

class WorldGridManager~T~{
    IWorldGridXZ~T~ Grid
    + WorldGridManager<T> ApplyValidator(params IGridValidation~T~[] gridValidations)
    + void ResetValidation()
}

class UnitWorldGridManager {
    UnitWorldGridValidator Validator()
}

WorldGridManager~T~ <|-- UnitWorldGridManager : implements

class IGridValidation~CellValue~ {
    <<interface>>
    + bool IsAvailable(GridCellXZ<CellValue> cell)
}

class IGridValidationIntent {
    <<interface>>
    + void Validate(ref UnitWorldGridValidator validator)
}

class GridUnitAction {
    + ApplyValidation()
}

class UnitActionsFactory

GridUnitAction ..> IGridValidationIntent

IGridValidationIntent --|> InteractableValidation
IGridValidationIntent --|> DirectDamageValidationIntent
IGridValidationIntent --|> InRangeValidationIntent
IGridValidationIntent --|> IsEmptyValidationIntent
IGridValidationIntent --|> NoValidationIntent
IGridValidationIntent --|> UnitInRangeValidationIntent

UnitActionsFactory --> InteractableValidation : instantiate
UnitActionsFactory --> DirectDamageValidationIntent : instantiate
UnitActionsFactory --> InRangeValidationIntent : instantiate
UnitActionsFactory --> IsEmptyValidationIntent : instantiate
UnitActionsFactory --> NoValidationIntent : instantiate
UnitActionsFactory --> UnitInRangeValidationIntent : instantiate
UnitActionsFactory --> GridUnitAction : instantiate

UnitWorldGridValidator <|-- InteractableValidation : build
UnitWorldGridValidator <|-- DirectDamageValidationIntent : build
UnitWorldGridValidator <|-- InRangeValidationIntent : build
UnitWorldGridValidator <|-- IsEmptyValidationIntent : build
UnitWorldGridValidator <|-- NoValidationIntent : build
UnitWorldGridValidator <|-- UnitInRangeValidationIntent : build

UnitWorldGridValidator <|-- GridUnitAction : Apply

GridUnitAction --> InteractableValidation : inject(UnitWorldGridValidator)
GridUnitAction --> DirectDamageValidationIntent : inject(UnitWorldGridValidator)
GridUnitAction --> InRangeValidationIntent : inject(UnitWorldGridValidator)
GridUnitAction --> IsEmptyValidationIntent : inject(UnitWorldGridValidator)
GridUnitAction --> NoValidationIntent : inject(UnitWorldGridValidator)
GridUnitAction --> UnitInRangeValidationIntent : inject(UnitWorldGridValidator)

UnitWorldGridValidator --> WorldGridManager : ApplyValidator(IGridValidation[])
UnitWorldGridManager --> UnitWorldGridValidator : instantiate
UnitWorldGridValidator --> IGridValidation : instantiate

UnitActionsFactory ..> UnitWorldGridManager

```
