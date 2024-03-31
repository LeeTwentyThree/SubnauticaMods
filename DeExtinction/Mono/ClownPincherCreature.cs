using System.Collections.Generic;
using UnityEngine;

namespace DeExtinction.Mono;

public class ClownPincherCreature : Creature
{
    public ClownPincherNibble nibble;
    public ClownPincherScavengeBehaviour scavengeBehaviour;
    public AnimateByVelocity animateByVelocity;

    public static readonly List<TechType> ClownPincherFoods = new List<TechType>()
    {
        TechType.SeaTreaderPoop,
        TechType.NutrientBlock,
        TechType.StalkerEgg,
        TechType.StalkerEggUndiscovered,
        TechType.GasopodEgg,
        TechType.GasopodEggUndiscovered,
        TechType.RabbitrayEgg,
        TechType.RabbitrayEggUndiscovered
    };
    
    public override void Start()
    {
        base.Start();
        Hunger.Value = Random.value;
    }

    public void PlayEatAnimation()
    {
        animateByVelocity.EvaluateRandom();
        GetAnimator().SetTrigger("eat");
    }
}