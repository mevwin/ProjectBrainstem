An entity is defined as anything that interacts with the game world or can be interacted with. Examples include: the Player character, enemies, movable objects, projectiles, items. So long as it executes some behavior upon interaction (so long as it does something), it is an entity.

Treat this class as a blueprint for making things with some explicit behavior. The main idea is that through inheritance/polymorphism, specific entity types can be derived from this base class. The benefit of this is that every single entity type will perform some base logic, on top of their specific logic. For example, what if we want every entity to not rotate. The redundant way is to copy-paste that one line of code into every entity's `Update` or `FixedUpdate` function. With this setup, all you need to write the line of code only once--within the base Entity class's `Update` function. 

Therefore, any changes made to the base Entity class will trickle down to every entity type, while any change made to one entity type won't affect anything else. This reduces copy-pasting significantly and ensures that we have the best of both worlds: general and specialized behaviors. For example, every entity may need to move. The logic to set/update their current movement vector can be written in the base Entity class. Regardless of the entity type, its movement vector will always be updated. However, the Player entity class will trigger (or calculate) movement differently from the NPC entity class. Each class' logic can be set within their respective set of scripts, but they can still use the base Entity's logic for actually updating and setting the values. This is done using `base` class method calls and virtual functions.

To make a new entity type:
1) Make a prefab variant (NOT original prefab) of the Entity prefab
2) Make a script where the class inherits from Entity (ex: `public class NewEntity : Entity`)
3) For the new script, override the following virtual functions and ensure the first line of each function contains `base.FunctionName()`to call the original function from the base Entity class:
	1) `Start`
	2) `Update`
	3) `FixedUpdate`
4) Override the `InitializeStates`function to add states to the entity. If no states have been made yet for the entity, make the states and update this function later.

NOTE: The entity's behavior is managed by a state machine. State machines are covered in`Classes/State Machine`.
