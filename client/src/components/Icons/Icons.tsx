// icons.tsx
import EducationIcon from '../../assets/icons/education.svg?react';
import ProfileIcon from '../../assets/icons/profile.svg?react';
import AddIcon from '../../assets/icons/add.svg?react';
import AddUserIcon from '../../assets/icons/addUser.svg?react';
import BoardIcon from '../../assets/icons/board.svg?react';
import CalendarIcon from '../../assets/icons/calendar.svg?react'
import BoardsIcon from '../../assets/icons/boards.svg?react';
import CheckIcon from '../../assets/icons/check.svg?react';
import ClockIcon from '../../assets/icons/clock.svg?react';
import DownloadIcon from '../../assets/icons/download.svg?react';
import DropDownIcon from '../../assets/icons/dropDown.svg?react';
import GraphicIcon from '../../assets/icons/graphic.svg?react';
import HashtagIcon from '../../assets/icons/hashtag.svg?react';
import HomeIcon from '../../assets/icons/home.svg?react';
import ListIcon from '../../assets/icons/list.svg?react';
import SearchIcon from '../../assets/icons/search.svg?react';
import TimelineIcon from '../../assets/icons/timeline.svg?react';
import UsersIcon from '../../assets/icons/users.svg?react';
import ShowPasswordIcon from '../../assets/icons/showPassword.svg?react'
import HidePasswordIcon from '../../assets/icons/hidePassword.svg?react'
import KeterProfileIcon from '../../assets/profile-icons/keterProfile.svg?react'
import SparkIcon from '../../assets/icons/spark.svg?react'
import NavHomeIcon from '../../assets/nav-icons/navHome.svg?react'
import NavTasksIcon from '../../assets/nav-icons/navTasks.svg?react'
import NavProjectsIcon from '../../assets/nav-icons/projectsNav.svg?react'
import NavNotificationIcon from '../../assets/nav-icons/navNotification.svg?react'


// Type
export type IconProps = React.SVGProps<SVGSVGElement> & {
  className?: string;
};

// Icon components
export const Education = (props: IconProps) => <EducationIcon className='icon' {...props} />;
export const Profile = (props: IconProps) => <ProfileIcon className='icon' {...props} />;
export const Add = (props: IconProps) => <AddIcon className='icon' {...props} />;
export const AddUser = (props: IconProps) => <AddUserIcon className='icon' {...props} />;
export const Board = (props: IconProps) => <BoardIcon className='icon' {...props} />;
export const Boards = (props: IconProps) => <BoardsIcon className='icon' {...props} />;
export const Check = (props: IconProps) => <CheckIcon className='icon' {...props} />;
export const Clock = (props: IconProps) => <ClockIcon className='icon' {...props} />;
export const Download = (props: IconProps) => <DownloadIcon className='icon' {...props} />;
export const DropDown = (props: IconProps) => <DropDownIcon className='icon' {...props} />;
export const Graphic = (props: IconProps) => <GraphicIcon className='icon' {...props} />;
export const Hashtag = (props: IconProps) => <HashtagIcon className='icon' {...props} />;
export const Home = (props: IconProps) => <HomeIcon className='icon' {...props} />;
export const List = (props: IconProps) => <ListIcon className='icon' {...props} />;
export const Search = (props: IconProps) => <SearchIcon className='icon' {...props} />;
export const Timeline = (props: IconProps) => <TimelineIcon className='icon' {...props} />;
export const Users = (props: IconProps) => <UsersIcon className='icon' {...props} />;
export const ShowPassword = (props: IconProps) => <ShowPasswordIcon className='icon' {...props}/>
export const HidePassword = (props: IconProps) => <HidePasswordIcon className='icon' {...props}/>
export const Calendar = (props: IconProps) => < CalendarIcon className="icon" {...props}/>
export const KeterProfile = (props:IconProps) => <KeterProfileIcon className="icon" {...props}/>
export const Spark = (props:IconProps) => <SparkIcon className="icon"{...props}/>
export const NavHome = (props:IconProps) => <NavHomeIcon className='icon'{...props}/>
export const NavTasks = (props:IconProps) => <NavTasksIcon className='icon'{...props}/>
export const NavProjects = (props:IconProps) => <NavProjectsIcon className='icon' {...props}/>
export const NavNotification = (props:IconProps) => <NavNotificationIcon className='icom'{...props}/>

// IconPack object
export const IconPack = {
  Education,
  Profile,
  Add,
  AddUser,
  Board,
  Boards,
  Check,
  Clock,
  Download,
  DropDown,
  Graphic,
  Hashtag,
  Home,
  List,
  Search,
  Timeline,
  Users,
  Calendar,
  ProfileIcon,
  Spark,
  NavHome,
  NavTasks,
  NavProjects,
  NavNotification
};

// Type
export type IconName = keyof typeof IconPack;

// Minimal gallery component
export const IconGallery = (props: IconProps) => (
  <>
    {Object.entries(IconPack).map(([name, Icon]) => (
      <Icon key={name} {...props} />
    ))}
  </>
);
