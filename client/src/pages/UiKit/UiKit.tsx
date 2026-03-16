import React from "react";
import { Button } from "../../components/Button/Button";
import InputData from "../../components/InputData/InputData";
import { IconGallery, Add, Download } from "../../components/Icons/Icons";
import styles from "./UIKit.module.scss";

export const UIKit = () => {
  return (
    <div className={styles.uiKitContainer}>
      <header className={styles.header}>
        <h1>Design System & UI Kit</h1>
        <p>A reference guide for all reusable components in the application.</p>
      </header>
      <section className={styles.section}>
        <h2>1. Buttons</h2>
        <div className={styles.componentGroup}>
          
          <div className={styles.row}>
            <h3>Variants</h3>
            <div className={styles.flexBox}>
              <Button variant="primary">Primary Button</Button>
              <Button variant="secondary">Secondary Button</Button>
              <Button variant="ghost">Ghost Button</Button>
            </div>
          </div>

          <div className={styles.row}>
            <h3>Sizes</h3>
            <div className={styles.flexBox}>
              <Button size="sm">Small (sm)</Button>
              <Button size="md">Medium (md)</Button>
              <Button size="lg">Large (lg)</Button>
            </div>
          </div>

          <div className={styles.row}>
            <h3>With Icons</h3>
            <div className={styles.flexBox}>
              <Button icon={<Add />} iconPosition="left">Left Icon</Button>
              <Button icon={<Download />} iconPosition="right" variant="secondary">Right Icon</Button>
              <Button icon={<Add />} size="sm" /> {/* Icon only */}
            </div>
          </div>

          <div className={styles.row}>
            <h3>States</h3>
            <div className={styles.flexBox}>
              <Button disabled>Disabled Primary</Button>
              <Button variant="secondary" disabled>Disabled Secondary</Button>
            </div>
          </div>
        </div>
      </section>

      <hr className={styles.divider} />

      <section className={styles.section}>
        <h2>2. Form Inputs (InputData)</h2>
        <div className={styles.gridBox}>
          
          <InputData 
            type="text" 
            id="text-input" 
            label="Standard Text Input" 
            placeholder="e.g. John Doe" 
          />
          
          <InputData 
            type="email" 
            id="email-input" 
            label="Email Address" 
            placeholder="john@example.com" 
          />
          
          <InputData 
            type="password" 
            id="password-input" 
            label="Password Input" 
          />
          
          <InputData 
            type="phone" 
            id="phone-input" 
            label="Phone Number" 
          />
          
          <InputData 
            type="calendar" 
            id="calendar-input" 
            label="Date Picker (Calendar)" 
          />
          
          <InputData 
            type="select" 
            id="select-input" 
            label="Custom Select Dropdown" 
          />
          
          <div className={styles.fullWidth}>
            <InputData 
              type="textarea" 
              id="textarea-input" 
              label="Text Area" 
              placeholder="Write a message..."
            />
          </div>

          <div className={styles.fullWidth}>
            <InputData 
              type="radio" 
              id="radio-input" 
              label="Radio Options" 
              options={["First Option", "Second Option", "Third Option"]}
            />
          </div>

        </div>
      </section>

      <hr className={styles.divider} />

      {/* --- ICONS SECTION --- */}
      <section className={styles.section}>
        <h2>3. Iconography</h2>
        <p className={styles.subtitle}>Rendered via the <code>IconGallery</code> component.</p>
        <div className={styles.iconGrid}>
          <IconGallery />
        </div>
      </section>
      
    </div>
  );
};