using SNCreationKitData.OptionGeneration.Attributes;

namespace SNCreationKitData.OptionGeneration.OptionTypeInterfaces;

public interface IDropDownGenerator<TContainer> : IPropertyOptionGenerator<DropDownAttribute, TContainer>;