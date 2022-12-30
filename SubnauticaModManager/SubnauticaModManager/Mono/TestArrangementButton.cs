using System.IO;

namespace SubnauticaModManager.Mono;

internal class TestArrangementButton : MonoBehaviour
{
    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(() => OnClick());
    }

    private void OnClick()
    {
        var menu = ModManagerMenu.main;
        if (menu == null) return;
        menu.prompt.Ask(
                "Test?",
                new PromptChoice("Yes", () => Test())
            );       
    }

    private void Main()
    {
        
    }

    private void Test()
    {
        Main();

        ModManagerMenu.main.prompt.Ask(
               "Success.",
               new PromptChoice("Close")
           );
    }
}
