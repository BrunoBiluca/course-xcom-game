# Documento de diagramas <!-- omit from toc --> 

> 💡 Objetivo do documento: documentar em formato de diagrama os sistemas implementados.
> Esse documento deve descrever de forma diagramal as **interações que cada interface e classes** presentes do sistema utilizam, de forma a ter uma visão clara das estruturas e **propiciar futuras otimizações e refatoração de código**.

Cada diagrama descrito nesse documento deve exibir apenas as interfaces, classes e métodos pertinentes ao sistema descrito.

Summary

- [Validação do Grid](#validação-do-grid)
- [Sistema de criação de intenções e ações](#sistema-de-criação-de-intenções-e-ações)
- [Grid manager validation system](#grid-manager-validation-system)
  - [Novo sistema de validação do grid dependendo a intenção que a unidade quer desempenhar](#novo-sistema-de-validação-do-grid-dependendo-a-intenção-que-a-unidade-quer-desempenhar)

# Validação do Grid

```mermaid
classDiagram

class IUnitWorldGridManager {
    <<interface>>
    Validator()
    ResetValidation()
}

class WorldGridView {
    Display()
    Update()
}

WorldGridView ..> IUnitWorldGridManager : update

class IGridIntent {
    <<interface>>
    GridValidation()
}
IGridIntent ..> IUnitWorldGridManager : call Validator()

UnitActionsFactory --> IGridIntent : instantiate
```

# Sistema de criação de intenções e ações

Instanciação de intenções quando um ator é selecionado e então uma intenção é associada.

```mermaid
classDiagram
direction LR

class UnitActionsView

class UnitActionsFactory {
    + GridUnitAction Get(UnitActionsEnum action)
}

class IAPActor {
    <<interface>>
    event Action OnCantExecuteAction;
    event Action OnActionFinished;

    void Execute();
    void Set(TIntent action);
    void UnsetAction();
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
IAPIntent --> IIntent : extends

class IGridIntent {
    <<interface>>
}
IGridIntent --> IAPIntent : extends


class InteractIntent
InteractIntent --> IGridIntent : implements

class MoveIntent
InteractIntent --> IGridIntent : implements

class ShootIntent
ShootIntent --> IGridIntent : implements

class SpinIntent
SpinIntent --> IGridIntent : implements

class ThrowGrenadeIntent
ThrowGrenadeIntent --> IGridIntent : implements

class MeleeAttackIntent
MeleeAttackIntent --> IGridIntent : implements

UnitActionsView --> UnitActionsFactory : call Get(UnitActionsEnum action)
UnitActionsView <-- UnitActionsFactory : IIntent instance
UnitActionsView --> IAPActor : call Set(IIntent)

UnitActionsFactory --> MoveIntent : instantiate(UnitActionsEnum)
UnitActionsFactory --> SpinIntent : instantiate(UnitActionsEnum)
UnitActionsFactory --> ShootIntent : instantiate(UnitActionsEnum)
UnitActionsFactory --> ThrowGrenadeIntent : instantiate(UnitActionsEnum)
UnitActionsFactory --> MeleeAttackIntent : instantiate(UnitActionsEnum)
UnitActionsFactory --> InteractIntent : instantiate(UnitActionsEnum)

```

Instanciação da ação que será executada pelo Ator. Quando o ator já está selecionado e uma intenção foi atribuida a ele, então é criada uma ação que será a execução da intenção selecionada pelo jogador para aquele ator.

```mermaid
classDiagram

class StepMovementAction
class SpinUnitAction
class ShootAction
class ThrowGrenadeAction
class MeleeAttackAction
class InteractAction

StepMovementAction --> IAction : implements
SpinUnitAction --> IAction : implements
ShootAction --> IAction : implements
ThrowGrenadeAction --> IAction : implements
MeleeAttackAction --> IAction : implements
InteractAction --> IAction : implements

IAPActor --> InteractIntent : call Create()
IAPActor --> MoveIntent : call Create()
IAPActor --> ShootIntent : call Create()
IAPActor --> SpinIntent : call Create()
IAPActor --> ThrowGrenadeIntent : call Create()
IAPActor --> MeleeAttackIntent : call Create()

MoveIntent --> StepMovementAction : instantiate(DependencyContainer)
SpinIntent --> SpinUnitAction : instantiate(DependencyContainer)
ShootIntent --> ShootAction : instantiate(DependencyContainer)
ThrowGrenadeIntent --> ThrowGrenadeAction : instantiate(DependencyContainer)
MeleeAttackIntent --> MeleeAttackAction : instantiate(DependencyContainer)
InteractIntent --> InteractAction : instantiate(DependencyContainer)

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