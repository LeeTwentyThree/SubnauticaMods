using SNCreationKitData.OptionGeneration.Attributes;

namespace SNCreationKitData.OptionGeneration.OptionTypeInterfaces;

public interface IClassGenerator<TContainer> : IPropertyOptionGenerator<SubClassAttribute, TContainer>;