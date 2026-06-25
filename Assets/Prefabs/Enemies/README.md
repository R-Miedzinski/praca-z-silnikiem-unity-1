# Enemies — instrukcja tworzenia i konfiguracji

## Architektura

```
Unit (MonoBehaviour, abstrakcyjny)
└── Enemy (abstrakcyjny) — logika AI oparta na grafie stanów
    └── MeleeEnemy — konkretny wróg walczący w zwarciu
```

```
EnemyState (abstrakcyjny) — węzeł grafu stanów
├── PatrolState
├── ChaseState
└── AttackState
```

| Skrypt | Lokalizacja |
|---|---|
| `Unit` | `Assets/Systems/UnitsSystem/Unit.cs` |
| `Enemy` | `Assets/Systems/EnemySystem/Enemy.cs` |
| `EnemyState` + stany | `Assets/Systems/EnemySystem/EnemyState.cs`, `States/` |
| `EnemySpawner` | `Assets/Systems/EnemySystem/EnemySpawner.cs` |
| `MeleeEnemy` | `Assets/Prefabs/Enemies/MeleeEnemy/MeleeEnemy.cs` |

---

## Krok 1 — Tworzenie prefaba MeleeEnemy

### 1. Utwórz pusty GameObject w scenie

W **Hierarchy**: `prawy przycisk → Create Empty`, nazwij go `MeleeEnemy`.

### 2. Dodaj komponenty w Inspektorze

Zaznacz obiekt w Hierarchy, w Inspektorze klikaj `Add Component`:

| Komponent | Uwagi |
|---|---|
| `MeleeEnemy` (skrypt) | plik: `Assets/Prefabs/Enemies/MeleeEnemy/MeleeEnemy.cs` |
| `Rigidbody 2D` | Gravity Scale = 0, Freeze Rotation Z = true |
| `Capsule Collider 2D` | hitbox wroga |
| `Sprite Renderer` | przypisz sprite lub użyj wbudowanego (patrz niżej) |

> **Tymczasowy sprite:** w polu `Sprite` kliknij kółko i wybierz `Knob` lub `Square` (wbudowane Unity). Ustaw **Color** na czerwony żeby odróżnić od gracza.

### 3. Ustaw wartości w Inspektorze

**Unit** (podstawowe staty):

| Pole | Przykład |
|---|---|
| Unit Name | `Melee Enemy` |
| Max Health | `50` |
| Movement Speed | `3` |
| Base Damage | `10` |
| Cooldown Reduction | `0` |
| Armor | `0` |

**Enemy** (logika AI):

| Pole | Przykład | Opis |
|---|---|---|
| Detection Range | `8` | zasięg wykrycia gracza |
| Attack Range | `1.5` | zasięg ataku |
| Patrol Radius | `5` | promień patrolowania wokół punktu spawnu |
| Patrol Waypoint Tolerance | `0.3` | odległość przy której uznajemy dotarcie do punktu |

**MeleeEnemy**:

| Pole | Przykład | Opis |
|---|---|---|
| Attack Cooldown | `1.5` | bazowy czas między atakami (modyfikowany przez Cooldown Reduction z Unit) |
| Attack Effects | patrz niżej | lista efektów aplikowanych przy ataku |

#### Konfiguracja Attack Effects

W polu **Attack Effects** dodaj wpis:
- **Effect Id** — wybierz `DealDamage` z listy
- **Effect Params → Value** — mnożnik obrażeń (np. `1` = 100% Base Damage)

Możesz dodać więcej efektów (np. `Slow`) żeby wróg przy ataku jednocześnie spowalniał gracza.

### 4. Zapisz jako prefab

Przeciągnij obiekt z **Hierarchy** do folderu `Assets/Prefabs/Enemies/MeleeEnemy/` w panelu **Project**.

Pojawi się niebieska ikona sześcianu — to jest prefab. Usuń oryginał ze sceny.

---

## Krok 2 — Podpięcie EnemySpawner do sceny

Skrypt: `Assets/Systems/EnemySystem/EnemySpawner.cs`

### 1. Utwórz obiekt Spawnera

W **Hierarchy**: `prawy przycisk → Create Empty`, nazwij `EnemySpawner`.

Dodaj komponent `EnemySpawner`.

### 2. Przypisz prefab wroga

W Inspektorze `EnemySpawnera` przeciągnij prefab `MeleeEnemy` z `Assets/Prefabs/Enemies/MeleeEnemy/` do pola **Enemy Prefab**.

### 3. Utwórz punkty spawnu

W Hierarchy utwórz child obiekty pod `EnemySpawner`:

```
EnemySpawner
├── SpawnPoint1
├── SpawnPoint2
└── SpawnPoint3
```

Dla każdego: `prawy przycisk na EnemySpawner → Create Empty`. Ustaw ich pozycję (Transform) w miejscach gdzie mają pojawiać się wrogowie.

### 4. Przypisz punkty spawnu w Inspektorze

W Inspektorze `EnemySpawnera`:
- Ustaw **Size** tablicy `Spawn Points` na liczbę punktów (np. `3`)
- Przeciągnij każdy `SpawnPoint` do odpowiedniego slotu

### 5. Czas respawnu

Pole **Respawn Delay** (domyślnie `5` sekund) — czas po którym wróg odrodzi się po śmierci.
Ustaw `0` żeby wyłączyć respawn.

---

## Graf stanów AI

Stany są klasami dziedziczącymi po `EnemyState`. Graf przejść budowany jest w `Enemy.BuildStateGraph()`.

```
Patrol ──(gracz w Detection Range)──► Chase
Chase  ──(gracz poza Detection Range)─► Patrol
Chase  ──(gracz w Attack Range)──────► Attack
Attack ──(gracz za daleko)───────────► Chase
```

Warunki przejść to lambdy przekazywane przez `AddTransition()` — nie są hardcoded w stanach.

---

## Dodawanie nowych typów wrogów

1. Utwórz nowy skrypt dziedziczący po `Enemy`
2. Zaimplementuj metodę `PerformAttack()` — jest wymagana (abstrakcyjna)
3. Opcjonalnie nadpisz `BuildStateGraph()` żeby zmienić graf stanów (np. usunąć Patrol, dodać nowy stan)
4. Zbuduj prefab według kroków z Kroku 1, używając nowego skryptu zamiast `MeleeEnemy`

### Przykład — wróg bez patrolu

```csharp
public class RangedEnemy : Enemy
{
    protected override EnemyState BuildStateGraph()
    {
        var chase = new ChaseState();
        var attack = new AttackState();

        chase.AddTransition(attack, () => /* w zasięgu strzału */);
        attack.AddTransition(chase, () => /* za daleko */);

        return chase;
    }

    public override void PerformAttack() { /* logika strzału */ }
}
```

### Dodawanie nowego stanu

1. Utwórz klasę dziedziczącą po `EnemyState` w `Assets/Systems/EnemySystem/States/`
2. Zaimplementuj `Execute(Enemy enemy)`
3. Podepnij stan do grafu przez `AddTransition()` w `BuildStateGraph()` wybranego wroga
