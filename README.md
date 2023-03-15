# Teachers Control
My project is a Blazor application for the daily attendance information of faculty external to the organization of a High School.
The class or section calendars are complex to coordinate between them and the staff designs them in Excel and then uploads each to a sharepoint list that the staff accesses with their calendar view.
![excelCalendarList](https://user-images.githubusercontent.com/19351006/225450649-885bf18d-5878-42a7-9b7c-986ec7dff01d.png)
![CalendarList](https://user-images.githubusercontent.com/19351006/225450927-8a953827-df9b-4b0e-825d-99e998dd61c7.png)

That system does not allow you to see on the calendar which teachers will be attending because it does not support the calendar view of the sharepoint list plus data.
The application made gets the data from the calendars in sharepoint and the teachers included in an AzureAD security group.
![TeachersPage](https://user-images.githubusercontent.com/19351006/225450883-a61eb2b4-d37d-41d5-9115-7197b50bcac8.png)

The application allows to make assignments of teachers to the corresponding sections and subjects, and these are saved in a sharepoint list for this purpose.
![addAssignmentPage](https://user-images.githubusercontent.com/19351006/225451099-e75cd7b5-3843-4cc9-aae7-5a586b6fd509.png)
![assignmentspage](https://user-images.githubusercontent.com/19351006/225451130-2c32e397-d086-431f-8eee-3d8fbe15a10c.png)

Finally, the application has a teacher control page, where you choose a date and according to this a list of teachers attending that day is displayed, in addition to their schedules, section, and subject taught.
![TeachersAssistanceControlPage](https://user-images.githubusercontent.com/19351006/225451251-12699e02-a24f-42f7-839d-525f81d69c68.png)

