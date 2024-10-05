using SNCreationKitData.OptionGeneration.Attributes;

namespace SNCreationKitData.OptionGeneration.OptionTypeInterfaces;

public interface ITupleGenerator<TContainer> : IPropertyOptionGenerator<ListAttribute, TContainer>;