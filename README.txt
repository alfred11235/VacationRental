Considerations about the changes made on the VacationRental project:

- The code was refactored. 
	- A repository to separate the logic from the controllers was added. 
	- The names and locations of the models were changed. The objects for receiving and sending data were renamed as "Resource". The objects for storing the data were separated from the Resource objects.
	- References between the objects of the model were added in addition to the use of the dictionaries. With this change, the way in which tables of a database are mapped was recreated.
	- The Mapping Nuget/functionality is used for returning the right fields on the endpoints. Some fields were added but the existing fields kept the original names for the properties.
	- The project was updated to use the .Net core 3.1. Some dependencies were changed beacause of this.
	- The enpoints use IActionResult to be able to return http status codes along with the corresponding message/data.
	
- The functionalities requested were implemented.
	- The logic for booking was changed. A new entity called UnitInformation was added. This entity allows to keep personalized information from each unit in a separated way. Also the bookings are now assigned to specific units inside a rental.
	- The PreparationTimeInDays field was added to the rental entity and was taken in consideration for booking and returning calendar information.
	- The new list PreparationTimes was added. This list contains the numbers of the units that are on the preparation time period on a given day.
	- The funcionality for modifying rentals was implemented. This endpoint checks if the new PreparationTimeInDays can be set without bookings been overlapped. Also it checks if new units should be added or removed. If that is the case the units removed are the ones that don't have bookings ongoing (considering the current datetime).
	