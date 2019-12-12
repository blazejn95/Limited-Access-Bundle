This is simulation of limited access bundle that uses Erlang model and different classes of traffic and optionally different priorities for each class (multi service). If You are not familiar with traffic engineering subjects such as:
- Poisson arrival process
- Erlang model
- Markov chains (memoryless stochastic processes)
etc.
I suggest to skip trying to understand what the code actually does.

If you are still reading this the simulation is basically an extension from one bundle(server) to multiple subbundles so for example instead of one bundle with 100 allocation units free we have ten subbundles with 10 AU each. How does this change anything? Well we can use this to simulate a cluster of servers etc. The rule is that every demand(request, packet, call it however you want) has to be fully serviced by ONE subbundle and cannot be split between multiple subbundles.
Now having the basic knowledge we can move onto how to set the input and how to read output:
- create three input files in either Debug or Release depending on which one you want to run: 
  - inputA - file that specifies the amount of subbundles and their AU size. Example file has four subbundles 30 AUs each
  - inputB - contains the amount of allocation units needed per class. Example file is already in the Debug folder filled with 1 2 6 separated with new line which means there are three classes 1st requiring 1 AU 2nd requiring 2 AUs and 3rd requiring 6 AUs 
  - prios - filled with numbers starting from 0. They describe priority of each class. Example file is filled with 0 1 2 separated with new lines which means 1st class has priority 0, 2nd class has priority 1 and 3rd class has priority 2. The lower the priority number the more important the class is. When the subbundles are filled and request of class 1 cannot be serviced then a class 2 demand that is ALREADY BEING HANDLED BY SYSTEM will be removed and the traffic will be lost.
