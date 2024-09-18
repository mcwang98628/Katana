using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSavageController : PlayerController
{
    public override bool IsCanMove => base.IsCanMove && 
                                      !((PlayerSavageAttack)roleAttack).IsAccepting &&
                                      !((PlayerSavageAttack)roleAttack).IsCounterattacking;
    
    
}
