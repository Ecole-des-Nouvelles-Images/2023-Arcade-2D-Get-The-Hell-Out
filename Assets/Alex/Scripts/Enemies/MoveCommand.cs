using UnityEngine;

namespace Alex.Scripts.Enemies
{
    [CreateAssetMenu(fileName = "New Move Command", menuName = "Move Command")]
    public class MoveCommand : Command
    {
        public Vector2 _destination; // Renseigne la destination 
        public float speed; // Add some speed to the Enemy's own. 
        
        private Vector2 _startingPosition;

        public override void Setup(Enemy enemy) {
            enemy.Memory[(this, "startingPosition")] = (Vector2) enemy.transform.position;
        }
        
        public override void Execute(Enemy enemy)
        {
            if (_startingPosition == Vector2.zero) _startingPosition = enemy.transform.position;
            //Vector2 currentDestination = _startingPosition + _destination - (Vector2)enemy.transform.position; // recalculer la destination à chaque frame à partir de la position de départ 
            Vector2 direction = _destination.normalized; // si on ne normalise pas, il baisse la vitesse à l'approche de la destination sans jamais l'atteindre
            enemy.transform.Translate(direction * ((enemy.EnemySpeed +speed) * Time.deltaTime));
        }

        public override bool IsFinished(Enemy enemy)
        {
            Vector2 startingPosition = (Vector2) enemy.Memory[(this, "startingPosition")];
            Vector2 currentDestination = startingPosition + _destination;
            float distance = Vector2.Distance(currentDestination, enemy.transform.position);
            //float normalizedDirection = (currentDestination - (Vector2) enemy.transform.position).Length();
            //Debug.Log(normalizedDirection);
            return distance <= 1f;
        }

        public override void CleanUp(Enemy enemy) {
            enemy.Memory.Remove((this, "startingPosition"));
        }
    }
}