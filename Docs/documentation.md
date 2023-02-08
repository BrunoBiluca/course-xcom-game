# Documentação <!-- omit from toc --> 

- [Unidades do jogo](#unidades-do-jogo)
  - [Diagrama de interfaces](#diagrama-de-interfaces)
  - [Diagrama de implementações](#diagrama-de-implementações)
- [Validação do grid](#validação-do-grid)
- [Criação de intenções e ações](#criação-de-intenções-e-ações)
- [Sistema de criação de intenções e ações](#sistema-de-criação-de-intenções-e-ações)

# Unidades do jogo

Cada unidade é uma entidade que é adicionada ao grid e é mapeada pelo criado. As unidades interagem umas com as outras por meio de cada um de seus respectivas funcionalidades.

Cada tipo de de unidade apresenta uma interface específica para os tipos de funcionalidades que elas implementam.

## Diagrama de interfaces

```mermaid
classDiagram
direction BT

class IUnit {
    <<interface>>

    string Name
    bool IsBlockable
    ITransform Transform
    UnitsFactions Faction
    ISelectable Selectable
}

class IDestroyableUnit {
    <<interface>>
    void Destroy()
}
IDestroyableUnit --> IUnit : extends

class IDamageableUnit {
    <<interface>>
    IDamageable Damageable
    IHealthSystem HealthSystem
}
IDamageableUnit --|> IUnit : extends

class ICharacterUnit {
    <<interface>>
    IAPActor Actor
    UnitConfig UnitConfig
    IAnimatorController AnimatorController

    ITransform RightShoulder
    ITransform ProjectileStart

    INavegationAgent TransformNav
    SoundEffects SoundEffects
    ISoundEffectsController SoundEffectsController
}
ICharacterUnit --> IDamageableUnit : extends

class IInteractableUnit {
    <<interface>>
    void Interact()
}
IInteractableUnit --> IUnit : extends
```

## Diagrama de implementações

```mermaid
classDiagram
direction BT

class IUnit {
    <<interface>>
}
class IDestroyableUnit {
    <<interface>>
}
IDestroyableUnit --> IUnit : extends

class IDamageableUnit {
    <<interface>>
}
IDamageableUnit --|> IUnit : extends

class ICharacterUnit {
    <<interface>>
}
ICharacterUnit --> IDamageableUnit : extends

class IInteractableUnit {
    <<interface>>
}
IInteractableUnit --> IUnit : extends

BlockadeUnit --|> IUnit : implements

DestroyableUnit --|> IDestroyableUnit : implements

PlayerUnit --|> ICharacterUnit : implements
ENemyUnit --|> ICharacterUnit : implements

```

# Validação do grid

A validação do grid é acionada no momento que o player seleciona uma intenção para um ator selecionado.

Nesse momento o grid é validado para exibir as posições disponíveis dada a intenção selecionada.

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

# Criação de intenções e ações

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
