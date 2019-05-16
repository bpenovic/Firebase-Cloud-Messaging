// Replace the following with your app's Firebase project configuration
const firebaseConfig = {
  apiKey: "AIzaSyCA3jKj6fVgjExAelLodz_6JCzRu5janss",
  authDomain: "dd2019-ccf43.firebaseapp.com",
  databaseURL: "https://dd2019-ccf43.firebaseio.com",
  projectId: "dd2019-ccf43",
  storageBucket: "dd2019-ccf43.appspot.com",
  messagingSenderId: "45562318462",
  appId: "1:45562318462:web:b89ccd3744ec70a9"
};

// Initialize Firebase
firebase.initializeApp(firebaseConfig);

// Retrieve Firebase Messaging object.
const messaging = firebase.messaging();

messaging.requestPermission().then(function () {
  console.log('Notification permission granted.');
  messaging.getToken().then(function (currentToken) {
    if (currentToken) {
      console.log('Current token: ', currentToken);
      SaveToken(currentToken);
    } else {
      console.log('No Instance ID token available. Request permission to generate one.');
    }
  }).catch(function (err) {
    console.log('An error occurred while retrieving token. ', err);
  });
}).catch(function (err) {
  console.log('Unable to get permission to notify.', err);
  });

// Callback fired if Instance ID token is updated.
messaging.onTokenRefresh(function () {
  messaging.getToken().then(function (refreshedToken) {
    console.log('Token refreshed:', refreshedToken);
    SaveToken(refreshedToken);
  }).catch(function (err) {
    console.log('Unable to retrieve refreshed token ', err);
  });
});

// Handle incoming messages. Called when:
// - a message is received while the app has focus
// - the user clicks on an app notification created by a service worker
//   `messaging.setBackgroundMessageHandler` handler.
messaging.onMessage(function (payload) {
  console.log('Message received. ', payload);
  // ...
});

function SaveToken(tokenValue) {
  $.post(saveTokenUrl, { token: tokenValue });
}