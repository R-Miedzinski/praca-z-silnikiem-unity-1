# Enemies — instrukcja tworzenia i konfiguracji

## Architektura

```
Unit (MonoBehaviour, abstrakcyjny)
└── Enemy (abstrakcyjny) — logika AI: Patrol / Chase / Attack
    └── MeleeEnemy — konkretny wróg walczący w zwarciu
```

Skrypty bazowe: `Assets/Prefabs/Enemies/Shared/Enemy.cs`, `Assets/Systems/UnitsSystem/Unit.cs`

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
| Attack Cooldown | `1.5` | czas między atakami (sekundy) |
| Damage Multiplier | `1` | mnożnik obrażeń relative do Base Damage |

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

## Diagram stanów AI (Enemy)

```
Patrol ──(gracz w Detection Range)──► Chase
Chase  ──(gracz poza Detection Range)─► Patrol
Chase  ──(gracz w Attack Range)──────► Attack
Attack ──(gracz za daleko)───────────► Chase
```

---

## Dodawanie nowych typów wrogów

1. Utwórz nowy skrypt dziedziczący po `Enemy`
2. Zaimplementuj metodę `OnAttack()` — jest wymagana (abstrakcyjna)
3. Opcjonalnie nadpisz `OnPatrol()` lub `OnChase()` jeśli chcesz zmienić zachowanie
4. Zbuduj prefab według kroków z Kroku 1, używając nowego skryptu zamiast `MeleeEnemy`
