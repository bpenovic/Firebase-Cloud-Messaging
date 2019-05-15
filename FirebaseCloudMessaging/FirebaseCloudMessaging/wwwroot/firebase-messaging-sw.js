self.importScripts('https://www.gstatic.com/firebasejs/6.0.2/firebase-app.js');
self.importScripts('https://www.gstatic.com/firebasejs/6.0.2/firebase-messaging.js');

const firebaseConfig = {
  apiKey: "AIzaSyCA3jKj6fVgjExAelLodz_6JCzRu5janss",
  authDomain: "dd2019-ccf43.firebaseapp.com",
  databaseURL: "https://dd2019-ccf43.firebaseio.com",
  projectId: "dd2019-ccf43",
  storageBucket: "dd2019-ccf43.appspot.com",
  messagingSenderId: "45562318462",
  appId: "1:45562318462:web:b89ccd3744ec70a9"
};

firebase.initializeApp(firebaseConfig);
const messaging = firebase.messaging();


messaging.setBackgroundMessageHandler(function (payload) {
  console.log('[firebase-messaging-sw.js] Received background message ', payload);
  const notificationTitle = payload.notification.title;
  const notificationOptions = {
    body: payload.notification.body,
    icon: payload.notification.icon,
    click_action: payload.notification.click_action
  };

  return self.registration.showNotification(notificationTitle,
    notificationOptions);
});