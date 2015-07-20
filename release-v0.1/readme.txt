  _                                                                                                               
 | |                                                                                                              
 | |_ ___   ___   ___  _ __ __ _ _ __   __ _  ___ _   _                                                           
 | __/ _ \ / _ \ / _ \| '__/ _` | '_ \ / _` |/ _ \ | | |                                                          
 | || (_) | (_) | (_) | | | (_| | | | | (_| |  __/ |_| |                                                          
  \__\___/_\___/ \___/|_| _\__,_|_| |_|\__, |\___|\__, | _____ _                  _____                     _     
       |  __ \(_)    | | (_)            __/ |      __/ ||_   _| |                / ____|                   | |    
  _   _| |  | |_  ___| |_ _  ___  _ __ |___/_ _ __|___/_  | | | |_ ___ _ __ ___ | (___   ___  __ _ _ __ ___| |__  
 | | | | |  | | |/ __| __| |/ _ \| '_ \ / _` | '__| | | | | | | __/ _ \ '_ ` _ \ \___ \ / _ \/ _` | '__/ __| '_ \ 
 | |_| | |__| | | (__| |_| | (_) | | | | (_| | |  | |_| |_| |_| ||  __/ | | | | |____) |  __/ (_| | | | (__| | | |
  \__,_|_____/|_|\___|\__|_|\___/|_| |_|\__,_|_|   \__, |_____|\__\___|_| |_| |_|_____/ \___|\__,_|_|  \___|_| |_|
                                                    __/ |                                                         
                                                   |___/                                                          

More info about this package in the following blog posts:

http://tooorangey.co.uk/posts/its-not-that-simple-this-dictionary-never-has-a-word-for-the-way-im-feeling/

&&

http://tooorangey.co.uk/posts/yes-i-see-how-it-works-but-what-is-the-dictionary-item-called-that-i-need-to-edit/

The dashboard, on install, is placed on both the translation and settings section of Umbraco,
you can change these locations in the dashboard.config file, your editors may not have access to either of these sections by default - you can grant access to them via their entry in the Users. A common usage is to grant access to the translations section but not settings; to allow editors to update dictinoary item translations but not access templates etc
