using SNCreationKitData.OptionGeneration.Attributes;

namespace SNCreationKitData.OptionGeneration.OptionTypeInterfaces;

public interface IListGenerator<TContainer> : IPropertyOptionGenerator<ListAttribute, TContainer>;