Listeners are entities that are able to receive event updates. Every entity is able to become a listener by extending ITriggerListener (i.e. `public Door : Interactable, ITriggerListener`).

Once an entity extends a listener, it can use the function OnTriggerEvent in order to detect a trigger event. The parameter for this function will either be TriggerEventType.Activated or TriggerEventType.Deactivated.

In order to connect a trigger with a listener, go to the interactable entity that you want to trigger your listener (e.g. button, lever, pressure plater). That interactable object will have a monobehaviour list for its listeners. Drag the instance of your listening entity into this list, this monnobehaviour needs to have extended the ITriggerListener in order to be properly triggered.
ctr: I want to update this system late to make it a bit easier to track trigger/listener combos

You can attach a listener to any number of triggers.

