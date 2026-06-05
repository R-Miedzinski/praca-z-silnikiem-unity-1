// Enum opisujący możliwe stany zachowania wroga.
// Maszyna stanów w Enemy.cs używa tych wartości do decydowania co wróg robi w danej chwili.
public enum EEnemyState
{
    Patrol,  // wróg wędruje po mapie (gracz nie został wykryty)
    Chase,   // wróg goni gracza (gracz jest w zasięgu detekcji)
    Attack   // wróg atakuje (gracz jest w zasięgu ataku)
}
