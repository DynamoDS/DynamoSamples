# Data Bridge Samples

The `Data Bridge` feature that Dynamo brings allows a semi-dynamic way to interact with your nodes by allowing you to register a callback that will be executed after your graph and by passing in the values received as input. This becomes interesting when developping custom UIs that are dependant from the input data.

A notable example would be the `Watch` node from Dynamo's core nodes. After the graph is ran, the `Watch` node shows up its new value on its UI by using the `Data Bridge` callback.

## Examples
### Packing Nodes
Packing nodes leverages the `Data Bridge` by proposing a way to bring in structured data and schema enforcing to Dynamo. Packing nodes will show you how you can redefine "dynamically" the `InPorts` and `OutPorts` of your node and how you can add custom validations to them. More info in the example's `README`.