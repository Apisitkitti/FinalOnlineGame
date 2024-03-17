public class PlayerSkill
{
  string skill;
  int clientId;
  string element;
  private string carreer;

  public PlayerSkill(int clientId, string carreer)
  {
    this.clientId = clientId;
    this.carreer = carreer;
  }

  public string getSkill()
  {
    return this.skill;
  }
  public int getClientId()
  {
    return this.clientId;
  }


}