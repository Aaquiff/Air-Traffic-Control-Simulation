# Air Traffic Control Simulation

Test project to learn
- WPF
- ASP Web Pages
- WCF Services

This is a distrbuted program that simulates Air Traffic Control(ATC) for airports. It tracks a set of planes flying along specific routes between these airports. Each ATC server could be deployed at airports and they will be coordinated by a master server. The simulation will work on 15-minute simulated time intervals, coordinated by a "master" server(15 simulated minutes, that is not real minutes). During each interval the "master contats the ATC("Slave") server to request that they update the simulation. These updates include tracking the position and fuel of all airplanes, deciding which planes need to be "handed over" to a different ATC server. (Hand-over between ATC servers occurs when a plan departing one airport approaches within 300km of its destination aiport.)
