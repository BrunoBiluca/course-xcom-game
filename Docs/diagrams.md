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
