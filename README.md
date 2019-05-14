# Firebase Cloud Messaging
Firebase Cloud Messaging workshop for DUMP Days 2019. conference

### Steps 
1. [Initial project with Identity](https://github.com/bpenovic/Firebase-Cloud-Messaging/tree/1.-Init-project-with-identity)
2. [Get and store user tokens](https://github.com/bpenovic/Firebase-Cloud-Messaging/tree/2.-Get-and-store-user-tokens) 
3. Create form for sending notifications
4. Handle sending notifications

## 2. Get and store user tokens
At the bottom of layout page add firebase scripts  

     <!-- Firebase JS SDKs -->
    <script src="https://www.gstatic.com/firebasejs/6.0.2/firebase-app.js"></script>
    <script src="https://www.gstatic.com/firebasejs/6.0.2/firebase-messaging.js"></script>
    <!----> 

Inside wwwroot/lib create new js file which will handle token.  

      const firebaseConfig = {
    };

    firebase.initializeApp(firebaseConfig);

    const messaging = firebase.messaging();

    messaging.requestPermission().then(function () {
      console.log('Notification permission granted.');
      messaging.getToken().then(function (currentToken) {
        if (currentToken) {
          console.log('Current token: ', currentToken);
        } else {
          console.log('No Instance ID token available. Request permission to generate one.');
        }
      }).catch(function (err) {
        console.log('An error occurred while retrieving token. ', err);
      });
    }).catch(function (err) {
      console.log('Unable to get permission to notify.', err);
    });

Inside Views/Home index.cshtml add Script section:

     @section Scripts {
         <script src="~/js/init-firebase.js"></script>
     }

**Now, when we start the application with https url, we should be asked for notifications and after that get token (See in console f12)**


Create new Database Model called Token, add-migration and update-database  

