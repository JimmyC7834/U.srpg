namespace Game.Unit
{
    /// <summary>
    /// An interface of object that listen to all the events of a unit object
    /// </summary>
    public partial interface IUnitEventsListener
    {
        /// <summary>
        /// Called when this object is registered to a unit
        /// </summary>
        partial void OnRegister();
        /// <summary>
        /// Called when this object is removed from a unit
        /// </summary>
        partial void OnRemove();
        /// <summary>
        /// Called when a unit's action phrase starts
        /// </summary>
        partial void OnActionStart();
        /// <summary>
        /// Called when a unit's action phrase ends
        /// </summary>
        partial void OnActionEnd();
        /// <summary>
        /// Called when a new turn starts
        /// </summary>
        partial void OnTurnStart();
        /// <summary>
        /// Called when a turn ends
        /// </summary>
        partial void OnTurnEnd();
        /// <summary>
        /// Called when a moment starts
        /// </summary>
        partial void OnMomentStart();
        /// <summary>
        /// Called when a moment ends
        /// </summary>
        partial void OnMomentEnd();
        /// <summary>
        /// Called just the attack happens
        /// </summary>
        /// <param name="info"> Info struct of the attack </param>
        partial void OnPreAttack(AttackInfo info);
        /// <summary>
        /// Called after the attack happened
        /// </summary>
        /// <param name="info"> Info struct of the attack </param>
        partial void OnPostAttack(AttackInfo info);
        /// <summary>
        /// Called when the attack missed
        /// </summary>
        /// <param name="info"> Info struct of the attack </param>
        partial void OnAttackMissed(AttackInfo info);
        /// <summary>
        /// Called when the attack is dodged by the target
        /// </summary>
        /// <param name="info"> Info struct of the attack </param>
        partial void OnAttackDodged(AttackInfo info);
        /// <summary>
        /// Called when the attack hits the target
        /// </summary>
        /// <param name="info"> Info struct of the attack </param>
        partial void OnAttackHit(AttackInfo info);
        /// <summary>
        /// Called just before this unit takes the attack
        /// </summary>
        /// <param name="info"> Info struct of the attack </param>
        partial void OnPreTakeAttack(AttackInfo info);
        /// <summary>
        /// Called after this unit took the attack
        /// </summary>
        /// <param name="info"> Info struct of the attack </param>
        partial void OnPostTakeAttack(AttackInfo info);
        /// <summary>
        /// Called when this unit dodged the attack
        /// </summary>
        /// <param name="info"> Info struct of the attack </param>
        partial void OnDodgeAttack(AttackInfo info);
        /// <summary>
        /// Called just before the unit takes any damage
        /// </summary>
        /// <param name="info"> Info struct of the damage </param>
        partial void OnPreTakeDamage(DamageInfo info);
        /// <summary>
        /// Called just before the unit takes any damage
        /// </summary>
        /// <param name="info"> Info struct of the damage </param>
        partial void OnPostTakeDamage(DamageInfo info);
    }
}