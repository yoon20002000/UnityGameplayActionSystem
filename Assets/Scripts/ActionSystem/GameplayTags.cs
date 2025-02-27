using System;

[Flags]
public enum GameplayTags
{
    None_Action                     = 0,
    Action_Attack                   = 1 << 0,
    Action_Deffence                 = 1 << 1,
    Action_Dash                     = 1 << 2,
    Action_Skill1                   = 1 << 3,
    Status_Attacking                = 1 << 4,
    Status_Defencing                = 1 << 5,
    Status_Stun                     = 1 << 6,
    Status_Invincibility            = 1 << 7,
    Status_Dashing                  = 1 << 8,
    Status_Bleeding                 = 1 << 9,
    Action_Skill2                   = 1 << 10,
}