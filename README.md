# CupkekGames Resources

Global / save-data-scoped player resources. Two independent sub-asmdefs: pick whichever you need.

## What's inside

- **`Currencies/`** (`CupkekGames.Resources.Currencies.asmdef`) — `Wallet` (id-keyed long balances with `Add` / `Spend` / `CanAfford`), `Currency` value type, `CurrencyDefinition` SO, `CurrencyCatalog`.
- **`Experiences/`** (`CupkekGames.Resources.Experiences.asmdef`) — `ExperienceTracker` (id-keyed long totals with `OnLevelUp` events), `Experience` value type, `ExperienceDefinition` SO, `ExperienceCatalog`, `ExperienceHelper` (static level math). Curves are registered separately:
  - `Curves/ExperienceCurveSO` — abstract base, override `GetRequiredExperience(int level)`.
  - `Curves/ExperienceCurveCatalog` — `AssetCatalog<ExperienceCurveSO>` registered under catalog id `"ExperienceCurve"`.
  - Three concrete curves shipped: `PolynomialExperienceCurveSO` (`offset + multiplier * level^power`), `LinearExperienceCurveSO`, `SteppedExperienceCurveSO` (designer-authored `int[]`).
  - `ExperienceDefinition.CurveKey` is a `CatalogKey` that points at any curve registered in the catalog. Multiple definitions can share one curve.

## Dependencies

- `com.cupkekgames.data` — `CatalogKey`, `IData`, `AssetCatalog<T>`
- `com.cupkekgames.services` — `ServiceProviderSO` for catalog registration

The two sub-asmdefs are independent — neither depends on the other. Reference only what you need.

## Installation

Embedded package — clone alongside the other CupkekGames packages.

## Related packages

- `com.cupkekgames.data` — foundation (CatalogKey, IData)
- `com.cupkekgames.rpgstats` — parallel pattern for per-unit combat attributes (`AttributeSet`)
- `com.cupkekgames.inventory` — parallel pattern for per-item stat bonuses (`ItemStatData`)
