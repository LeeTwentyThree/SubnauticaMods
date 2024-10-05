using SNCreationKitData.OptionGeneration.Attributes;

namespace SNCreationKitData.OptionGeneration.OptionTypeInterfaces;

public interface IInputFieldGenerator<TContainer> : IPropertyOptionGenerator<InputFieldAttribute, TContainer>;