Authors: Ben Huenemann and Jonathan Ryan Wigderson
PS8
Version: 1.0

This client creates a form with a controller that manages the connection that it has
with the server. This controller starts by sending the player name to the specified
server. Then the server sends the player ID and the world size back. After this it
starts an event loop where it receives information, processes the information,
invalidates the form so it can be redrawn, and then sends the commands to the server.
It analyzes the information sent by deserializing the JSON text. Then it stores the
information so it can be drawn later.

Whenever the form is invalidated, it redraws each element at specified location with
the given information. For the tanks and projectiles it draws it differently depending
on the color stored by the controller. The turret is drawn on top of the tank at the
same location pointing the direction of the mouse. It also spawns an explosion when
the health is equal to 0 that consists of particles radiating out from the center at
random angles For the beam it spawns particles surrounding the beam. These particles
move in random directions for a certain amount of time to create a cool animation. For
the walls it splits them into square segments and draws them individually. It also has
elements for the health and player names that are drawn near the tank.

There are also classes inside the model that represent each of these aspects and store
the serializable properties of those elements. There's also a static class that contains
the constants for how to draw each element.