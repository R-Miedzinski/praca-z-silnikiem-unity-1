public class AddItemToBackpackEffect : Effect, IParametrizedEffect
{
  private string itemId;

  public void SetParameters(EffectParamsData paramsData)
  {
    itemId = paramsData.StringValue;
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