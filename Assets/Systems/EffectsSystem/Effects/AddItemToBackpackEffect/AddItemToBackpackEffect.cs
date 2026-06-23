public class AddItemToBackpackEffect : Effect, IParametrizedEffect
{
  private string itemId;

  public void SetParameters(string[] paramsData)
  {
    if (paramsData.Length > 0)
    {
      itemId = paramsData[0];
    }
    else
    {
      EffectsUtils.InvalidParameters(0, "string (itemId)");
    }
  }

  public override void ApplyEffect(Unit caster, Unit target)
  {
    if (target is PlayerCharacter player)
    {
      Item itemToAdd = ItemsDatabase.Instance.GetItemById(itemId);
      player.AddItemToBackpack(itemToAdd);
    }
  }
}