# CupkekGames Resources — AI Agent Instructions

## Package Overview

**CupkekGames Resources** (`com.cupkekgames.resources`) is the global / save-data-scoped player-resources system. Currencies (Wallet) and Experiences (Tracker) live here as independent sub-asmdefs.

## Critical: Do not hand-edit Unity serialized assets or `.meta` files

Apply scene/SO changes in Unity Editor; preserve `.meta` GUIDs across moves.

## Package Structure

```
com.cupkekgames.resources/
  package.json
  README.md
  AGENTS.md
  Currencies/                    ← CupkekGames.Resources.Currencies.asmdef
    Runtime/
  Experiences/                   ← CupkekGames.Resources.Experiences.asmdef
    Runtime/
      Curves/                      (registered curve SOs + their catalog)
```

The two sub-asmdefs are deliberately independent. Don't add a reference between them.

## Dependencies

- `com.cupkekgames.data` — `CatalogKey`, `IData`, `AssetCatalog<T>`, `CatalogKeyConstraint`
- `com.cupkekgames.services` — `ServiceProviderSO`, `ServiceLocator`

## Coding Conventions

- `_camelCase` for private fields, `PascalCase` for public members
- `[SerializeField] private` with public property getter
- `using` statements grouped: System, Unity, CupkekGames, then file-specific
- Catalogs derive from `AssetCatalog<TSubject>` (defined in `com.cupkekgames.data`) and register themselves under a constant `CatalogId` string defined in a `*Constants.cs` static class
- Runtime state types (`Wallet`, `ExperienceTracker`) implement `IData`, are JSON-serializable, and fire events for UI binding
- Definition SOs carry display metadata (display name as `string`, not `LocalizedString` — let consumers wrap), icon, and any per-instance config (max amount, curve key, etc.)

## Naming Convention

Namespace pluralized to avoid collision with same-named class (`CupkekGames.Resources.Currencies` contains class `Currency`). Same rule for `Experiences`.

## Curve System

`ExperienceCurveSO` is an abstract base. Concrete curves (`PolynomialExperienceCurveSO`, `LinearExperienceCurveSO`, `SteppedExperienceCurveSO`) ship with the package. Games author additional subclasses as needed. All curves are registered in an `ExperienceCurveCatalog` (catalog id `"ExperienceCurve"`) and referenced by `CatalogKey` from `ExperienceDefinition.CurveKey`. Single source of truth — multiple definitions can share one curve asset.

## Adding a new currency or experience track

1. Create a `CurrencyDefinition` / `ExperienceDefinition` asset.
2. Drop it into the corresponding catalog asset.
3. Reference by id string (`"Gold"`, `"Hero"`) or `CatalogKey`.

No code changes needed.
