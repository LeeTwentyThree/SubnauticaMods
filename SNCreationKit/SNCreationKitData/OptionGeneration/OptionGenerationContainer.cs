using SNCreationKitData.OptionGeneration.OptionTypeInterfaces;

namespace SNCreationKitData.OptionGeneration;

public record OptionGenerationContainer<TContainer>(
    IUserInterfaceManager<TContainer> UserInterfaceManager,
    IClassGenerator<TContainer> ClassGenerator,
    IDropDownGenerator<TContainer> DropDownGenerator,
    IInputFieldGenerator<TContainer> InputFieldGenerator,
    IListGenerator<TContainer> ListGenerator,
    ISliderGenerator<TContainer> SliderGenerator,
    ITupleGenerator<TContainer> TupleGenerator)
{
    internal IUserInterfaceManager<TContainer> UserInterfaceManager { get; } = UserInterfaceManager;
    internal IClassGenerator<TContainer> ClassGenerator { get; } = ClassGenerator;
    internal IDropDownGenerator<TContainer> DropDownGenerator { get; } = DropDownGenerator;
    internal IInputFieldGenerator<TContainer> InputFieldGenerator { get; } = InputFieldGenerator;
    internal IListGenerator<TContainer> ListGenerator { get; } = ListGenerator;
    internal ISliderGenerator<TContainer> SliderGenerator { get; } = SliderGenerator;
    internal ITupleGenerator<TContainer> TupleGenerator { get; } = TupleGenerator;
}