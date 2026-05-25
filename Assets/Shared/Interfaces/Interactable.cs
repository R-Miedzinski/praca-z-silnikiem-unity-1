public interface IInteractable
{
  public bool InteractOnContact { get; }
  public void Interact(PlayerCharacter player);

  public void EnableHighlight();
  public void DisableHighlight();
}