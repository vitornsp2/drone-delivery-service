# drone-delivery-service

API written using .Net core 6 to take input of drones and location and find the most efficient deliveries for each drone.

## Run

1 - dotnet run 

2 - It'll open the swagger page

3 - upload the Input.text file

## Solution

First, Remove any packages that cant be carried.

Second, Drones list ordered from smallest weight limit to greatest and finds the most packages a the drone can carry.

Thrid, Find and return the greatest total of packages a weight can hold from list of packages

