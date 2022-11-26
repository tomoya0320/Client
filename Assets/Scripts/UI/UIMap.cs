namespace GameCore.UI {
  public class UIMap : UIBase {
    public async void Close() {
      await UIManager.Instance.Close(this);
    }
  }
}