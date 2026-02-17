Interactable entities are entities that the player can interact with to change the state (e.g. pressure plate, lever, button). These entities can also send their state out as a event, either activated or deactivated. Other entities with listeners attached will be able to pick up on these event calls in order to change their own state (i.e. a door opening when a lever is flipped on).

To create a new interactable:
1) Assume same process for creating an entity, but instead extend Interactable (ex: `public class NewInteractable : Interactable`)
2) In NewInteractable script, override the DetectActivation() function and include base.DetectActivation() at the END of the the function. It must be at the end in order to properly notify listeners of the activation changes.
3) Change the logic in DetectActivation to be specific to your entity. Note: Event activation is dependent on the isActive variable of the Interactable script.
4) the isActive variable can be set through entity states (i.e. ButtonPressedState handles logic for when the button stops beinng pressed). When isActive is updated, the DetectActivation function should be called to send out the event

For receiving event calls, follow instructions in the Listeners tab.
   