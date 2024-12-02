<h1 align="center">Callejo Inc. Childcare Website</h1>

<p align="center">
  <img src="Company_logo.png" alt="Callejo Inc. Logo" style="width: 200px;">
</p>

## Project Synopsis
The Callejo Inc. Childcare Website is an ongoing senior project being developed by Tech Squad, a team of Computer Science students at California State University, Sacramento. Running from Fall 2024 to Spring 2025, this project aims to support Jane Callejo’s family-owned childcare business by creating a user-friendly and effective website.

Our goal is to make daily operations at Callejo Inc. smoother and more efficient. Right now, tasks like managing child rosters, scheduling staff, and keeping track of business expenses are done manually, which takes time and can lead to mistakes. By building this website, we hope to centralize these processes, making them easier to manage and reducing the chance for errors. This will ultimately save time for the staff and allow them to focus more on what really matters—caring for the children.

We’re also focusing on making communication with parents easier and clearer. The website will provide timely updates and notifications so parents stay informed and connected. Through these features, we aim to create a reliable and organized system that supports the childcare center’s operations and builds trust with families. We're currently in the process of developing the website and are committed to delivering a tool that meets the needs of both the business and the parents it serves.

## Key Features
- **Feature 1**: User Experience
  - The website provides an easy to follow and visually appealing UI
  - Users can login to created accounts and stay up to date with their child's schedule/planner and track upcoming events
- **Feature 2**: Admin Controls
  - Admin users will have better business management tools via the web app
  - Admins can: observe and track business expenses, set and manage employee tasks, and manage customer accounts.
- **Feature 3**: Password Reset
  - Users are able to recover their password via the "Forgot Password" functionality.

## Project Status
The software engineering team developed a comprehensive web application involving three main components: the frontend, backend, and database. For the frontend, we used Blazor, a framework for building interactive web UI with C# in conjuction with JavaScript, to create a dynamic and responsive user interface. Our team also employed HTML, CSS, to design the structure, style, and interactivity of the application, ensuring an intuitive user experience. Blazor enabled us to write both client-side and server-side code in C#, providing a seamless and consistent approach to frontend development. The backend was implemented using C#, handling the logic and operations that support the frontend. It served as the bridge between the frontend and the database, ensuring data is processed and sent to the user interface. For the database layer, MS SQL (Microsoft SQL Server) was used, where data is stored, managed, and retrieved. The team set up tables, relationships, and queries to handle data persistence efficiently. The entire application, including the frontend, backend, and database, will  be deployed on Azure, which provides a cloud infrastructure to host the application, manage scalability, and ensure reliability.


## Images and Diagrams
### Mock Up (created in Jira)
<p align="center">
  <img src="childcare_figma.png" alt="figma" style="width: 800px;">
</p>

### ERD (Entity-Relationship Diagram)
<p align="center">
  <img src="childcare_ERD.png" alt="ERD" style="width: 600px;">
</p>

### Prototype Images
Welcome Page: 
<p align="left">
  <img src="visitors_page.png" alt="visitors_page" style="width: 500px;">
</p>

Personalized Customer Page: 
<p align="left">
  <img src="customer_page.png" alt="customer_page" style="width: 500px;">
</p>

## Timeline and Milestones
### Sprint 1
- Development and Planning

### Sprint 2
- Public Side: Create website landing pages that parents looking for childcare can navigate to

### Sprint 3
- Public Side: Create login page for parents whose children are enrolled in childcare, employees of the childcare, along with the owner/operator of the childcare
- Set up forgotten password structure
- Align edit profile page with client's needs
- Public Side: Please create necessary SQL tables to store interested parents
- Update placeholder text for Mission page

### Sprint 4
- Database Development: Form Control, API Integration, Data Retrieval
- Parents Side: Create UI and Database for the Enrolled Children Page
- Parents Side: Ability to upload registration paperwork to childcare owner/operator
- Refactor website to fix scaling issue
- Update placeholder text for rest of the landing pages
- Public Side: Contact Page Functionality
- Parents Side: Create UI for Parent Profile Page and Link it to Edit Profile page

### Sprint 5
- Public Side: Provide a mechanism to inform childcare operator/owner when parents register their interest in enrolling their kids.
- Parents Side: Provide functionality that displays notes and communications from the childcare provider/owner.
- Parents Side: Ability to view pictures and activities posted by childcare operator.
- Parents Side: Provide a way for parents to send a note to the childcare provider or owner (e.g., “My child is sick and will be absent”).
- Owners Side: Share pictures, plans, and activities with parents regarding their children in the daycare.

### Sprint 6
- Parents Side: Need the ability to view daily schedules entered by childcare operator.
- Parents Side: Need to be able to view observed holidays and childcare operators' paid vacations.
- Parents Side: Need to provide some type of acknowledgment mechanism when paperwork is received through the website.
- Owners Side: Functionality providing a roster of children enrolled in childcare.
- Owners Side: Ability to file registration paperwork by parent and child.

### Sprint 7
- Owners Side: Ability to provide paperwork to prospective parents.
- Owners Side: Create a data entry form so that parents and children in childcare can be entered into the system.
- Owners Side: Add/remove registration paperwork as needed.
- Owners Side: Need the ability to enter daily schedules for parents to view.
- Owners Side: Need the ability to enter observed holidays and paid vacation dates.

### Sprint 8
- Owners Side: Need the ability to enter business expenses and upload receipts.
- Owners Side: Need accounts receivable payment feature.
- Owners Side: Accounts receivable report feature.
- Parents Side: Backend integration for enrolled children management.

### Sprint 9
- Owners Side: Need the ability to upload and make available required information pamphlets for registration paperwork.
- Deployment: Provide a landing environment for the website.

## Testing
*Placeholder for testing details to be filled in CSC 191.*

## Deployment
*Placeholder for deployment steps and considerations to be completed in CSC 191.*

## Developer Instructions
*Placeholder for developer setup instructions including dependencies, installation, and configuration steps to be completed in CSC 191.*

## Future Enhancements

### 1. Expense Tracking and Reporting
- **Expense Tracking**: Add a feature in the admin panel for the childcare operator to log and manage business expenses.
- **Receipt Management**: Allow the operator to upload receipts for better financial tracking.
- **Expense Reporting**: Generate detailed reports (daily, weekly, and monthly) summarizing expenses.

### 2. Email Notification System
- Modularize our email system to allow childcare operators to send updates or notifications to parents and guardians:
  - Automated thank-you emails with attached paperwork for parents expressing interest in enrolling their children.
  - Customizable email templates for sending important announcements or reminders.

### 3. Authentication, Authorization, and State Management
- Introduce an authentication and authorization system to manage user roles (e.g., admin, parent, staff) and dynamically change website content based on the logged-in state and user role.
- Implement state management to maintain user sessions and improve user experience.

### 4. Enhanced Admin Panel
- Provide functionality in the admin panel to allow the operator to:
  - Add, remove, or update registration paperwork for parents and guardians.

## Contributors
**Team Tech Squad**
- Evan Callejo (Team Lead)
- Masoud Ahang
- Angel Calderon
- Wei Chong
- Jonathon Delemos
- Justin Ear
- Chris Iverson
- Alan Lei

*All team members contributed to both front-end and back-end development for this project.*

## Contact Information
- Masoud Ahang (Development Team):
    - masoudahang@csus.edu
- Angel Calderon (Development Team):
    - angelcalderon2@csus.edu
- Evan Callejo (Development Team):
    - evancallejo@csus.edu
- Wei Chong (Development Team):
    - weichong@csus.edu
- Jonathon Delemos (Development Team):
    - jdelemos@csus.edu
- Justin Ear (Development Team):
    - justinear@csus.edu
- Chris Iverson (Development Team):
    - civerson@csus.edu
- Alan Lei (Development Team):
    - alei2@csus.edu
- Samuel and Jane Callejo (Clients):
    - samuelcallejo7406@gmail.com
    - samuelcallejo@callejoinc527.onmicrosoft.com
