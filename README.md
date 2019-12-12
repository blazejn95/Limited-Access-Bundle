This is simulation of limited access bundle that uses Erlang model and different classes of traffic (multi service). If You are not familiar with traffic engineering subjects such as:
- Poisson arrival process
- Erlang model
- Markov chains (memoryless stochastic processes)
etc.
I suggest to skip trying to understand what the code actually does.

If you are still reading this the simulation is basically an extension from one bundle(server) to multiple subbundles so for example instead of one bundle with 100 allocation units free we have ten subbundles with 10 AU each. How does this change anything? Well we can use this to simulate a cluster of servers etc. The rule is that every demand has to be fully serviced by ONE subbundle and cannot be split between multiple subbundles.
Now having the basic knowledge we can move onto how to set teh input and how to read output...........
