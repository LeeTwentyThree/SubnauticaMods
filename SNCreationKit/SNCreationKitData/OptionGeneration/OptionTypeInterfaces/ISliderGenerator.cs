using SNCreationKitData.OptionGeneration.Attributes;

namespace SNCreationKitData.OptionGeneration.OptionTypeInterfaces;

public interface ISliderGenerator<TContainer> : IPropertyOptionGenerator<SliderAttribute, TContainer>;