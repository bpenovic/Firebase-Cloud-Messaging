# Firebase Cloud Messaging
Firebase Cloud Messaging workshop for DUMP Days 2019. conference

### Steps 
1. [Initial project with Identity](https://github.com/bpenovic/Firebase-Cloud-Messaging/tree/1.-Init-project-with-identity)
2. [Get and store user tokens](https://github.com/bpenovic/Firebase-Cloud-Messaging/tree/2.-Get-and-store-user-tokens) 
3. [Create form and send notifications](https://github.com/bpenovic/Firebase-Cloud-Messaging/tree/3.-Create-form-for-sending-notifications)  

## Create form and send notifications  
First of all create form for sending notifications and controller. Then add method for sending notifications and validating response. Also, create models for response, inject httpclient and appsettings.  


**firebase-messaging-sw.js**
  self.importScripts('https://www.gstatic.com/firebasejs/6.0.2/firebase-app.js');
  self.importScripts('https://www.gstatic.com/firebasejs/6.0.2/firebase-messaging.js');

  const firebaseConfig = {
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
