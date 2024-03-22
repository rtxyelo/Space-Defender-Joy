using System.Collections;

public interface IBonusesController
{
    void ShieldBonusOn();
    void DeathRayBonusOn();
    IEnumerator ShieldRoutine();
    IEnumerator DeathRayRoutine();
}
