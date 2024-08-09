using UnityEngine;
using UnityEngine.UIElements;

public class SettingsPanel : MonoBehaviour
{
    public VisualElement root;
    public VisualElement settingsPanel;

    // Particle Arena
    public Slider arenaHeightSlider;
    public Slider arenaDiameterSlider;

    // Enscribed Disc
    public Slider discRotationSpeedSlider;
    public TextField discText;
    public Slider discTextSizeSlider;

    // Universal Machine Settings
    public Slider forceScalingSlider;
    // ... add other scaling sliders ...

    void Start()
    {
        // Get the root visual element of your UI Document
        root = GetComponent<UIDocument>().rootVisualElement;

        // Create the settings panel
        settingsPanel = new VisualElement();
        settingsPanel.name = "SettingsPanel";

        // Add styling (optional)
        // settingsPanel.style.backgroundColor = Color.gray;
        // settingsPanel.style.padding = 10;

        // Create UI elements
        // Particle Arena
        arenaHeightSlider = new Slider();
        arenaHeightSlider.name = "ArenaHeightSlider";
        arenaHeightSlider.style.width = 300;
        arenaHeightSlider.lowValue = 10;
        arenaHeightSlider.highValue = 100;
        arenaHeightSlider.value = 50;

        arenaDiameterSlider = new Slider();
        arenaDiameterSlider.name = "ArenaDiameterSlider";
        arenaDiameterSlider.style.width = 300;
        arenaDiameterSlider.lowValue = 10;
        arenaDiameterSlider.highValue = 100;
        arenaDiameterSlider.value = 50;

        // Enscribed Disc
        discRotationSpeedSlider = new Slider();
        discRotationSpeedSlider.name = "DiscRotationSpeedSlider";
        discRotationSpeedSlider.style.width = 300;
        discRotationSpeedSlider.lowValue = 1;
        discRotationSpeedSlider.highValue = 10;
        discRotationSpeedSlider.value = 5;

        discText = new TextField();
        discText.name = "DiscText";
        discText.value = "Contact Theory";

        discTextSizeSlider = new Slider();
        discTextSizeSlider.name = "DiscTextSizeSlider";
        discTextSizeSlider.style.width = 300;
        discTextSizeSlider.lowValue = 1;
        discTextSizeSlider.highValue = 10;
        discTextSizeSlider.value = 5;

        // Universal Machine Settings
        forceScalingSlider = new Slider();
        forceScalingSlider.name = "ForceScalingSlider";
        forceScalingSlider.style.width = 300;
        forceScalingSlider.lowValue = 0.1f;
        forceScalingSlider.highValue = 10f;
        forceScalingSlider.value = 1f;

        // ... add other scaling sliders ...

        // Add elements to the panel
        settingsPanel.Add(new Label("Particle Arena Settings"));
        settingsPanel.Add(arenaHeightSlider);
        settingsPanel.Add(arenaDiameterSlider);

        settingsPanel.Add(new Label("Enscribed Disc Settings"));
        settingsPanel.Add(discRotationSpeedSlider);
        settingsPanel.Add(discText);
        settingsPanel.Add(discTextSizeSlider);

        settingsPanel.Add(new Label("Universal Machine Settings"));
        settingsPanel.Add(forceScalingSlider);
        // ... add other scaling sliders ...

        // Add the panel to the root
        root.Add(settingsPanel);
    }

    // Add methods to handle UI events, for example:

    public void OnArenaHeightChanged(ChangeEvent<float> evt)
    {
        // Update the height of the particle arena
        // ... (your code here) ...
    }

    public void OnArenaDiameterChanged(ChangeEvent<float> evt)
    {
        // Update the diameter of the particle arena
        // ... (your code here) ...
    }

    // ... add methods for other UI elements ...
}